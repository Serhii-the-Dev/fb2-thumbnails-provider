using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using FBThumbs;

/**
 * Code below is based in Jeremy's article: 
 * http://www.codemonkeycodes.com/2010/01/11/ithumbnailprovider-re-visited/
 */

namespace Thumbnailer
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    [ProgId("FB2.ThumbnailProvider"), Guid("c1c9ecac-4eff-4358-8cf0-01b600d85ff4")]
    public class ThumbnailProvider : IThumbnailProvider, IInitializeWithStream
    {
        #region IInitializeWithStream

        protected IStream BaseStream
        {
            get { return (_stream); }
            set { this._stream = value; }
        }

        public void Initialize(IStream stream, int grfMode)
        {
            this.BaseStream = stream;            
        }

        #endregion

        #region IThumbnailProvider

        public void GetThumbnail(int cx, out IntPtr hBitmap, out WTS_ALPHATYPE bitmapType)
        {
            hBitmap = IntPtr.Zero;
            bitmapType = WTS_ALPHATYPE.WTSAT_UNKNOWN;
            FB2FileReader FB2File;
            try
            {
                //using (Bitmap Thumbnail = new Bitmap(cx, cx))
                //{
                    FB2File = new FB2FileReader();
                    byte[] vInput = GetStreamContents();
                    MemoryStream stream = new MemoryStream(vInput);
                    int size = FB2File.Read(vInput); //read your file 
                    if(size > 0)
                    {
                        //create your thumbnail                         
                        //GCHandle vPinnedArray = GCHandle.Alloc(vBuffer, GCHandleType.Pinned);
                        //IntPtr vPointer = vPinnedArray.AddrOfPinnedObject();
                        //Bitmap new_bitmap = new Bitmap(188, 141, 564, System.Drawing.Imaging.PixelFormat.Format24bppRgb, vPointer);
                        byte[] vBuffer = FB2File.GetBuffer();
                        if (vBuffer != null && vBuffer.Length > 0)
                        {
                            Bitmap vBitmap = new Bitmap(new MemoryStream(vBuffer));
                            hBitmap = vBitmap.GetHbitmap();
                        }
                        else
                            hBitmap = IntPtr.Zero;
                    }                    
                //}
            }
            catch
            {
                hBitmap = IntPtr.Zero;
            }
            finally
            {
                //I think this is the code that Ralfn was referencing, but this has NOT been tested
                _stream = null;
                FB2File = null;
                GC.Collect();
            }

        }

        protected IStream Stream
        {
            get { return (_stream); }
            set { this._stream = value; }
        }
        /// <summary>
        /// The ThumbnailProvider returns a stream that we convert to a byte array.
        /// </summary>
        /// <returns></returns>
        protected byte[] GetStreamContents()
        {
            System.Diagnostics.Debug.Assert(this._stream != null);

            // How big do we think this stream is?
            System.Runtime.InteropServices.ComTypes.STATSTG statData;
            _stream.Stat(out statData, 1); // 1 means don't bother with the name.

            // Allocate some memory.
            byte[] bytes = new byte[statData.cbSize];
            IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(UInt64)));

            this._stream.Read(bytes, bytes.Length, p);

            Marshal.FreeCoTaskMem(p);

            return (bytes);
        }

        private IStream _stream;

        #endregion
    }
}


namespace Thumbnailer
{

    /// <summary>
    /// Defines the format of a bitmap returned by an <see cref="IThumbnailProvider"/>.
    /// </summary>
    public enum WTS_ALPHATYPE
    {
        /// <summary>
        /// The bitmap is an unknown format. The Shell tries nonetheless to detect whether the image has an alpha channel.
        /// </summary>
        WTSAT_UNKNOWN = 0,
        /// <summary>
        /// The bitmap is an RGB image without alpha. The alpha channel is invalid and the Shell ignores it.
        /// </summary>
        WTSAT_RGB = 1,
        /// <summary>
        /// The bitmap is an ARGB image with a valid alpha channel.
        /// </summary>
        WTSAT_ARGB = 2,
    }

    /// <summary>
    /// Exposes a method for getting a thumbnail image.
    /// </summary>
    [ComVisible(true), Guid("e357fccd-a995-4576-b01f-234630154e96"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IThumbnailProvider
    {
        /// <summary>
        /// Retrieves a thumbnail image and alpha type. 
        /// </summary>
        /// <param name="cx">The maximum thumbnail size, in pixels. The Shell draws the returned bitmap at this size or smaller. The returned bitmap should fit into a square of width and height <paramref name="cx"/>, though it does not need to be a square image. The Shell scales the bitmap to render at lower sizes. For example, if the image has a 6:4 aspect ratio, then the returned bitmap should also have a 6:4 aspect ratio.</param>
        /// <param name="hBitmap">When this method returns, contains a pointer to the thumbnail image handle. The image must be a device-independent bitmap (DIB) section and 32 bits per pixel. The Shell scales down the bitmap if its width or height is larger than the size specified by cx. The Shell always respects the aspect ratio and never scales a bitmap larger than its original size.</param>
        /// <param name="bitmapType">Specifies the format of the output bitmap.</param>
        void GetThumbnail(int cx, out IntPtr hBitmap, out WTS_ALPHATYPE bitmapType);
    }

    /// <summary>
    /// Provides a method used to initialize a handler, such as a property handler, thumbnail provider, or preview handler, with a file stream.
    /// </summary>
    [ComVisible(true), Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithStream
    {
        /// <summary>
        /// Initializes a handler with a file stream.
        /// </summary>
        /// <param name="stream">Pointer to an <see cref="IStream"/> interface that represents the file stream source.</param>
        /// <param name="grfMode">Indicates the access mode for <paramref name="stream"/>.</param>
        void Initialize(IStream stream, int grfMode);
    }

}
