﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SimpleBudgetFreshMvvm.PlatformServices;
using System.IO;
using SimpleBudgetFreshMvvm.Config;
using SimpleBudgetFreshMvvm.Droid.PlatformServices;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace SimpleBudgetFreshMvvm.Droid.PlatformServices
{
    public class FileService : IFileService
    {
        public string DbPath
        {
            get
            {
                var personalDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                return Path.Combine(personalDir, Consts.DbName);
            }
        }
    }
}