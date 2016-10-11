using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Itis.Common
{
    public static class FileUtil
    {
        private static int _clusterSize;
        private static readonly object ClusterSizeLockObject = new object();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "GetDiskFreeSpaceW")]
        static extern bool GetDiskFreeSpace(string lpRootPathName, out int lpSectorsPerCluster, out int lpBytesPerSector, out int lpNumberOfFreeClusters, out int lpTotalNumberOfClusters);

        // Each partition has its own cluster size.
        public static int GetClusterSize()
        {
            return GetClusterSize(null);
        }

        public static int GetClusterSize(string path)
        {

            int sectorsPerCluster;
            int bytesPerSector;
            int freeClusters;
            int totalClusters;
            int clusterSize = 0;
            if (GetDiskFreeSpace(Path.GetPathRoot(path), out sectorsPerCluster, out bytesPerSector, out freeClusters, out totalClusters))
                clusterSize = bytesPerSector * sectorsPerCluster;
            return clusterSize;
        }

        public static void Init()
        {
            InitClusterSize();
        }

        private static void InitClusterSize()
        {
            lock (ClusterSizeLockObject)
            {
                _clusterSize = GetClusterSize();
            }
        }

        public static int ClusterSize
        {
            get 
            {
                lock (ClusterSizeLockObject)
                {
                    return _clusterSize;
                }
            }
        }
    }
}
