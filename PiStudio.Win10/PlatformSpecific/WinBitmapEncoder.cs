﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiStudio.Shared;
using PiStudio.Shared.Data;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace PiStudio.Win10
{
    public class WinBitmapEncoder : IBitmapEncoder
    {
        private BitmapEncoder encoder;
        private WinBitmapEncoder() { }
        private async Task Initialize(Guid BitmapEncoderGuid, IRandomAccessStream stream)
        {
            encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);

            
        }
        public async Task SetPixelData(PixelFormat format, bool ignoreAlphaMode, uint pixelWidth, uint pixelHeight, double dpiX, double dpiY, byte[] pixels)
        {
            WriteableBitmap bitmap = new WriteableBitmap((int)pixelWidth, (int)pixelHeight);

            using (Stream stream = bitmap.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(pixels, 0, pixels.Length);
            }
            byte[] m_pixels;
            using (Stream stream = bitmap.PixelBuffer.AsStream())
            {//???????
                m_pixels = new byte[stream.Length];
                await stream.ReadAsync(m_pixels, 0, m_pixels.Length);
            }
            encoder.SetPixelData((BitmapPixelFormat)format,
                                 ignoreAlphaMode ? BitmapAlphaMode.Ignore : BitmapAlphaMode.Straight,
                                 pixelWidth,
                                 pixelHeight,
                                 dpiX,
                                 dpiY,
                                 m_pixels);
        }

        public static async Task<WinBitmapEncoder> CreateAsync(Stream stream, string fileFormat)
        {
            WinBitmapEncoder encoder = new WinBitmapEncoder();
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            switch (fileFormat)
            {
                case "jpeg":
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                    break;

                case "png":
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                    break;

                case "bmp":
                    BitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                    break;

                case "tiff":
                    BitmapEncoderGuid = BitmapEncoder.TiffEncoderId;
                    break;

                case "gif":
                    BitmapEncoderGuid = BitmapEncoder.GifEncoderId;
                    break;
            }
            await encoder.Initialize(BitmapEncoderGuid, stream.AsRandomAccessStream());
            return encoder;
        }

        public async Task FlushAsync()
        {
            await encoder.FlushAsync();
        }
    }
}
