﻿using System;
using System.IO;
using System.Threading.Tasks;
using PiStudio.Shared;
using PiStudio.Shared.Data;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace PiStudio.Win10
{
    public class WinBitmapDecoder : IBitmapDecoder
    {
        private Windows.Graphics.Imaging.BitmapDecoder decoder;
        private byte[] m_pixelData;

        private WinBitmapDecoder() { }

        private async Task InitializeAsync(IRandomAccessStream stream)
        {
            decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
            BitmapTransform transform = new BitmapTransform();
            PixelDataProvider provider = await decoder.GetPixelDataAsync(decoder.BitmapPixelFormat,
                                                                   BitmapAlphaMode.Straight,
                                                                   transform,
                                                                   ExifOrientationMode.IgnoreExifOrientation,
                                                                   ColorManagementMode.DoNotColorManage);
            m_pixelData = provider.DetachPixelData();
        }
        public PixelFormat PixelFormat
        {
            get
            {
                return (PixelFormat)decoder.BitmapPixelFormat;
            }
        }

        public uint PixelHeight
        {
            get
            {
                return decoder.PixelHeight;
            }
        }

        public uint PixelWidth
        {
            get
            {
                return decoder.PixelWidth;
            }
        }

        public double DpiX
        {
            get
            {
                return decoder.DpiX;
            }
        }

        public double DpiY
        {
            get
            {
                return decoder.DpiY;
            }
        }

        public static async Task<IBitmapDecoder> CreateAsync(Stream stream)
        {
            WinBitmapDecoder decoder = new WinBitmapDecoder();
            await decoder.InitializeAsync(stream.AsRandomAccessStream());
            return decoder;
        }

        public async Task<byte[]> GetPixelDataAsync()
        {
            return m_pixelData;
        }
    }
}
