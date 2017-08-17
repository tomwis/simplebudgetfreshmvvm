using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleBudgetFreshMvvm.Resources
{
    [ContentProperty("Text")]
    public class LocalizeExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return AppResources.ResourceManager.GetString(Text);
        }
    }
}
