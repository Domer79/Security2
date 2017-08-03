using System;
using System.IO;
using System.IO.Compression;

namespace Tools.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Копирует данные из одного потока в другой
        /// </summary>
        /// <param name="stream">Исходный поток</param>
        /// <param name="destStream">Поток назначения</param>
        /// <returns></returns>
        public static Stream CopyTo(this Stream stream, Stream destStream)
        {
            if (destStream == null) throw new ArgumentNullException("destStream");

            stream.Position = 0;
            var buf = new byte[8192];
            int read;

            while ((read = stream.Read(buf, 0, buf.Length)) > 0)
            {
                destStream.Write(buf, 0, read);
            }

            return destStream;
        }

        /// <summary>
        /// Сжимает данные (с помощью <see cref="GZipStream"/>) в потоке и копирует их в другой
        /// </summary>
        /// <param name="source">Исходный поток</param>
        /// <param name="dest">Поток назначения</param>
        /// <returns>Возвращает массив байтов сжатых данных</returns>
        public static byte[] Compress(this Stream source, Stream dest)
        {
            using (var zipStream = new GZipStream(dest, CompressionMode.Compress, true))
            {
                var buffer = new byte[8040]; //chunk sizes that are multiples of 8040 bytes.
                int read;

                if (!source.CanSeek)
                    throw new InvalidOperationException("Входной поток не поддерживает смещение позиции указателя чтения");

                if (!dest.CanSeek)
                    throw new InvalidOperationException("Выходной поток не поддерживает смещение позиции указателя чтения");

                source.Seek(0, SeekOrigin.Begin);

                while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    zipStream.Write(buffer, 0, read);
                }
            }

            var compressBuffer = new byte[dest.Length];
            dest.Seek(0, SeekOrigin.Begin);
            dest.Read(compressBuffer, 0, Convert.ToInt32(dest.Length));

            return compressBuffer;

        }

        /// <summary>
        /// Деархивирует данные в потоке и полученные данные копирует в поток назначения
        /// </summary>
        /// <param name="source">Исходный поток со сжатыми данными</param>
        /// <param name="dest">Поток назначения</param>
        public static void DeCompress(this Stream source, Stream dest)
        {
            using (var zipStream = new GZipStream(source, CompressionMode.Decompress))
            {
                var buffer = new byte[8040]; //chunk sizes that are multiples of 8040 bytes.
                int read;

                if (!source.CanSeek)
                    throw new InvalidOperationException("Входной поток не поддерживает смещение позиции указателя чтения");

                if (!dest.CanSeek)
                    throw new InvalidOperationException("Выходной поток не поддерживает смещение позиции указателя чтения");

                source.Seek(0, SeekOrigin.Begin);

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dest.Write(buffer, 0, read);
                }
                dest.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}