using SimpleBudgetFreshMvvm.Config;
using SimpleBudgetFreshMvvm.PlatformServices;
using SimpleBudgetFreshMvvm.UWP.PlatformServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace SimpleBudgetFreshMvvm.UWP.PlatformServices
{
    public class FileService : IFileService
    {
        public string DbPath
        {
            get
            {
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                return Path.Combine(localFolder.Path, Consts.DbName);
            }
        }
    }
}
