//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace PhotoLab
{
    public sealed partial class DetailPage : Page
    {
        ImageFileInfo item;
        Compositor compositor;
        CompositionEffectBrush combinedBrush;
        CultureInfo culture = CultureInfo.CurrentCulture;
        ContrastEffect contrastEffect;
        ExposureEffect exposureEffect;
        TemperatureAndTintEffect temperatureAndTintEffect;
        GaussianBlurEffect graphicsEffect;
        SaturationEffect saturationEffect;
        bool editingInitialized = false;
        bool canNavigateWithUnsavedChanges = false;

        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            item = e.Parameter as ImageFileInfo;
            canNavigateWithUnsavedChanges = false;
            ResetEffects();

            if (item != null)
            {
                item.PropertyChanged += (s, e2) => UpdateEffectBrush(e2.PropertyName);
                targetImage.Source = item.ImageSource;
                ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("itemAnimation");
                if (imageAnimation != null)
                {
                    imageAnimation.Completed += (s, e_) =>
                    {
                        MainImage.Source = item.ImageSource;
                        targetImage.Source = null;
                    };
                    imageAnimation.TryStart(targetImage);
                }
            }
            else
            {
                // error
            }

            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // If the photo has unsaved changes, we want to show a dialog
            // that warns the user before the navigation happens
            // To give the user a chance to view the dialog and respond,
            // we go ahead and cancel the navigation.
            // If the user wants to leave the page, we restart the
            // navigation. We use the canNavigateWithUnsavedChanges field to
            // track whether the user has been asked.
            if (item.NeedsSaved && !canNavigateWithUnsavedChanges)
            {
                // The item has unsaved changes and we haven't shown the
                // dialog yet. Cancel navigation and show the dialog.
                e.Cancel = true;
                ShowSaveDialog(e);
            }
            else
            {
                canNavigateWithUnsavedChanges = false;
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backAnimation", MainImage);
                base.OnNavigatingFrom(e);
            }
        }

        /// <summary>
        /// Gives users a chance to save the image before navigating
        /// to a different page.
        /// </summary>
        private async void ShowSaveDialog(NavigatingCancelEventArgs e)
        {
            ContentDialog saveDialog = new ContentDialog()
            {
                Title = "Unsaved changes",
                Content = "You have unsaved changes that will be lost if you leave this page.",
                PrimaryButtonText = "Leave this page",
                SecondaryButtonText = "Stay"
            };
            ContentDialogResult result = await saveDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // The user decided to leave the page. Restart
                // the navigation attempt. 
                canNavigateWithUnsavedChanges = true; 
                Frame.Navigate(e.SourcePageType, e.Parameter);
            }
        }

        private void ZoomSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MainImageScroller != null)
            {
                MainImageScroller.ChangeView(null, null, (float)e.NewValue);
            }
        }

        private void MainImageScroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ZoomSlider.Value = ((ScrollViewer)sender).ZoomFactor;
        }

        private void FitToScreen()
        {
            var zoomFactor = (float)Math.Min(MainImageScroller.ActualWidth / item.ImageSource.PixelWidth,
                MainImageScroller.ActualHeight / item.ImageSource.PixelHeight);
            MainImageScroller.ChangeView(null, null, zoomFactor);
        }

        private void ShowActualSize()
        {
            MainImageScroller.ChangeView(null, null, 1);
        }

        private void UpdateZoomState()
        {
            if (MainImageScroller.ZoomFactor == 1)
            {
                FitToScreen();
            }
            else
            {
                ShowActualSize();
            }
        }

        private void InitializeEffects()
        {
            saturationEffect = new SaturationEffect()
            {
                Name = "SaturationEffect",
                Saturation = item.Saturation,
                Source = new CompositionEffectSourceParameter("Backdrop")
            };
            contrastEffect = new ContrastEffect()
            {
                Name = "ContrastEffect",
                Contrast = item.Contrast,
                Source = saturationEffect
            };
            exposureEffect = new ExposureEffect()
            {
                Name = "ExposureEffect",
                Source = contrastEffect,
                Exposure = item.Exposure,
            };
            temperatureAndTintEffect = new TemperatureAndTintEffect()
            {
                Name = "TemperatureAndTintEffect",
                Source = exposureEffect,
                Temperature = item.Temperature,
                Tint = item.Tint
            };
            graphicsEffect = new GaussianBlurEffect()
            {
                Name = "Blur",
                Source = temperatureAndTintEffect,
                BlurAmount = item.Blur,
                BorderMode = EffectBorderMode.Hard,
            };
        }

        private void InitializeCompositor()
        {
            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            InitializeEffects();
            MainImage.Source = item.ImageSource;
            MainImage.InvalidateArrange();

            var destinationBrush = compositor.CreateBackdropBrush();

            var graphicsEffectFactory = compositor.CreateEffectFactory(graphicsEffect, new[] {
                "SaturationEffect.Saturation", "ExposureEffect.Exposure", "Blur.BlurAmount",
                "TemperatureAndTintEffect.Temperature", "TemperatureAndTintEffect.Tint",
                "ContrastEffect.Contrast" });
            combinedBrush = graphicsEffectFactory.CreateBrush();
            combinedBrush.SetSourceParameter("Backdrop", destinationBrush);

            var effectSprite = compositor.CreateSpriteVisual();
            effectSprite.Size = new Vector2((float)item.ImageSource.PixelWidth, (float)item.ImageSource.PixelHeight);
            effectSprite.Brush = combinedBrush;
            ElementCompositionPreview.SetElementChildVisual(MainImage, effectSprite);

            editingInitialized = true;
        }

        private void ToggleEditState()
        {
            if (MainSplitView.IsPaneOpen)
            {
                MainSplitView.IsPaneOpen = false;
            }
            else
            {
                if (!editingInitialized)
                { 
                    InitializeCompositor();
                }
                MainSplitView.IsPaneOpen = true;
            }
        }

        private void UpdateEffectBrush(string propertyName)
        {
            void update(string effectName, float effectValue) =>
                combinedBrush?.Properties.InsertScalar(effectName, effectValue);

            switch (propertyName)
            {
                case nameof(item.Exposure): update("ExposureEffect.Exposure", item.Exposure); break;
                case nameof(item.Temperature): update("TemperatureAndTintEffect.Temperature", item.Temperature); break;
                case nameof(item.Tint): update("TemperatureAndTintEffect.Tint", item.Tint); break;
                case nameof(item.Contrast): update("ContrastEffect.Contrast", item.Contrast); break;
                case nameof(item.Saturation): update("SaturationEffect.Saturation", item.Saturation); break;
                case nameof(item.Blur): update("Blur.BlurAmount", item.Blur); break;
                default: break;
            }
        }

        private async void ExportImage()
        {
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            using (CanvasRenderTarget offscreen = new CanvasRenderTarget(
                device, item.ImageSource.PixelWidth, item.ImageSource.PixelHeight, 96))
            {
                using (IRandomAccessStream stream = await item.ImageFile.OpenReadAsync())
                using (CanvasBitmap image = await CanvasBitmap.LoadAsync(offscreen, stream, 96))
                {
                    saturationEffect.Source = image;
                    using (CanvasDrawingSession ds = offscreen.CreateDrawingSession())
                    {
                        ds.Clear(Windows.UI.Colors.Black);

                        // Need to copy the value of each effect setting.
                        contrastEffect.Contrast = item.Contrast;
                        exposureEffect.Exposure = item.Exposure;
                        temperatureAndTintEffect.Temperature = item.Temperature;
                        temperatureAndTintEffect.Tint = item.Tint;
                        saturationEffect.Saturation = item.Saturation;
                        graphicsEffect.BlurAmount = item.Blur;
                        ds.DrawImage(graphicsEffect);
                    }

                    var fileSavePicker = new FileSavePicker()
                    {
                        SuggestedSaveFile = item.ImageFile
                    };

                    fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });

                    var outputFile = await fileSavePicker.PickSaveFileAsync();

                    if (outputFile != null)
                    {
                        using (IRandomAccessStream outStream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await offscreen.SaveAsync(outStream, CanvasBitmapFileFormat.Jpeg);
                        }

                        ResetEffects();
                        var newItem = await MainPage.LoadImageInfo(outputFile);

                        if (outputFile.Path == item.ImageFile.Path)
                        {
                            item.ImageSource = newItem.ImageSource; 
                        }
                        else
                        {
                            item = newItem;
                        }

                        MainImage.Source = item.ImageSource;
                    }
                }
            }
        }

        private void ResetEffects()
        {
            item.Exposure =
                item.Blur =
                item.Tint =
                item.Temperature =
                item.Contrast = 0;
            item.Saturation = 1;
        }

    }
}
