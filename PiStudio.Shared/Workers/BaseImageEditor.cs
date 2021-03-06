﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiStudio.Shared.Data;
using System.IO;

namespace PiStudio.Shared
{
    /// <summary>
    /// Represents base class for every image editor that want to process the image.
    /// Platform independent.
    /// </summary>
    public abstract class BaseImageEditor : IImageEditor
    {
        protected byte[] m_workingImageInBytes = null;
        protected byte[] m_unsavedImageInBytes = null;

        protected uint m_imageWidth;
        protected uint m_imageHeight;

        protected byte m_bytePerPixel;
        protected double m_dpiX;
        protected double m_dpiY;

        protected PixelFormat m_pixelFormat;

        public BaseImageEditor(string filepath)
        {
            int index = filepath.LastIndexOf('.');
            MimeType = filepath.Substring(index + 1);
            HasUnsavedChange = false;
        }

        /// <summary>
        /// Gets whether object has some unsaved changes
        /// </summary>
        public bool HasUnsavedChange { get; set; }

        /// <summary>
        /// Image resolution in X axis
        /// </summary>
        public uint PixelWidth
        {
            get { return m_imageWidth; }
        }

        /// <summary>
        /// Image resolution in Y axis
        /// </summary>
        public uint PixelHeight
        {
            get { return m_imageHeight; }
        }

        /// <summary>
        /// Image suffix. I.e. 'jpg'
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// How many bytes are per one pixel
        /// </summary>
        public byte PixelSize
        {
            get { return m_bytePerPixel; }
        }

        //return whether image contains alpha color
        private bool IsAlpha()
        {
            return m_bytePerPixel != 1;
        }

        /// <summary>
        /// Changes brightness of the image.
        /// </summary>
        protected byte[] ApplyBrightness(int brightness)
        {
            var brightnessBytes = ImageToolkit.ApplyBrightness(m_workingImageInBytes, m_bytePerPixel, IsAlpha(), brightness);
            m_unsavedImageInBytes = brightnessBytes;
            HasUnsavedChange = true;
            return brightnessBytes;
        }

		/// <summary>
		/// Changes brightness of the image.
		/// </summary>
		protected byte[] ApplyBrightness(byte[] imageInBytes, int brightness)
		{
			var brightnessBytes = ImageToolkit.ApplyBrightness(imageInBytes, m_bytePerPixel, IsAlpha(), brightness);
			return brightnessBytes;   
		}

        /// <summary>
        /// Applies kernel matrix on pixel data.
        /// </summary>
        protected byte[] ApplyFilter(Filter filter)
        {
            byte[] tmpPixels = ImageToolkit.ApplyConvolutionMatrixFilter(this.m_workingImageInBytes, (int)this.m_imageWidth,
               (int)this.m_imageHeight, filter.Matrix, (byte)m_bytePerPixel, IsAlpha(), filter.Factor, filter.Bias);

            byte[] resultPixels = tmpPixels;
            m_unsavedImageInBytes = resultPixels;
            HasUnsavedChange = false;
            return resultPixels;
        }

		/// <summary>
		/// Applies kernel matrix on pixel data.
		/// </summary>
		protected byte[] ApplyFilter(byte[] imageInBytes, Filter filter)
		{
			byte[] tmpPixels = ImageToolkit.ApplyConvolutionMatrixFilter(imageInBytes, (int)this.m_imageWidth,
	   			(int)this.m_imageHeight, filter.Matrix, (byte)m_bytePerPixel, IsAlpha(), filter.Factor, filter.Bias);
			return tmpPixels;   
		}

        /// <summary>
        /// Rotates given pixel data to the right.
        /// </summary>
        protected byte[] Rotate()
        {
            var rotatedBytes = ImageToolkit.Rotate(m_unsavedImageInBytes, m_imageWidth, m_imageHeight, m_bytePerPixel);
            var tmp = m_imageHeight;
            m_imageHeight = m_imageWidth;
            m_imageWidth = tmp;

            m_unsavedImageInBytes = rotatedBytes;
            HasUnsavedChange = true;
            return rotatedBytes;
        }

        /// <summary>
        /// Sets the raw image data
        /// </summary>
        public void SetSource(byte[] imageBytes)
        {
            if (imageBytes.Length != m_workingImageInBytes.Length)
                return;
            m_unsavedImageInBytes = imageBytes;
            HasUnsavedChange = true;
        }

        /// <summary>
        /// Saves inner unsaved changes
        /// </summary>
        public void SaveChanges()
        {
            m_unsavedImageInBytes.CopyTo(m_workingImageInBytes, 0);
            HasUnsavedChange = false;
        }

        /// <summary>
        /// Writes objects inner data into <see cref="IBitmapEncoder"/>
        /// </summary>
        /// <param name="encoder">encoder</param>
        public async Task WriteBytesToEncoder(IBitmapEncoder encoder)
        {
            encoder.SetPixelData(m_pixelFormat, false,
                                 m_imageWidth,
                                 m_imageHeight,
                                 m_dpiX,
                                 m_dpiY,
                                 m_workingImageInBytes);
            await encoder.FlushAsync();
        }

        /// <summary>
        /// Save content into given stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="suffix">file type</param>
        public abstract Task Save(Stream stream, string suffix);

        /// <summary>
        /// Dismiss unsaved changes.
        /// </summary>
        public void Dismiss()
        {
            m_workingImageInBytes.CopyTo(m_unsavedImageInBytes, 0);
            HasUnsavedChange = false;
        }
    }
}
