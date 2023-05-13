using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ConvenienceStoreManagement.Main.ViewModel;
using System;
using System.Globalization;
using System.Reflection;

namespace ConvenienceStoreManagement.Tools
{
    public class BitmapAssetValueConverter : IValueConverter
    {
        public static BitmapAssetValueConverter Instance = new();

        private static string assemblyName = Assembly.GetEntryAssembly().GetName().Name;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (value is string rawUri && targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                Uri uri;

                try
                {
                    if (rawUri == "")
                    {
                        return null;
                    }
                    else if (rawUri.StartsWith("avares://"))
                    {
                        uri = new Uri($"avares://{assemblyName}/{rawUri[9..]}");
                    }
                    else if (rawUri.StartsWith("storage://"))
                    {
                        return new Bitmap(ItemManageViewModel.ImageFolder + rawUri[10..]);
                    }
                    else
                    {
                        return new Bitmap(rawUri);
                    }

                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var asset = assets.Open(uri);

                    return new Bitmap(asset);
                }
                catch
                {
                    uri = new Uri($"avares://{assemblyName}/Assets/NotFound.png");
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var asset = assets.Open(uri);
                    return new Bitmap(asset);
                }

            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
