using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryMethod
{
    public interface ITheme
    {
        string TextColor { get; }
        string BgrColor { get; }
    }

    public class DarkTheme : ITheme
    {
        public string TextColor => "white";

        public string BgrColor => "black";
        
    }
    public class LightTheme : ITheme
    {
        public string TextColor => "black";
        public string BgrColor => "white";
        
    }
    public class TrackingThemeFactory
    {
        private readonly List<WeakReference<ITheme>> themes = 
            new List<WeakReference<ITheme>>();
        public ITheme CreateThemem(bool light)
        {
            ITheme theme;
            if (light)
                theme = new LightTheme();
            else
                theme = new DarkTheme();
            themes.Add(new WeakReference<ITheme>(theme));
            return theme;
        }
        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach(var reference in themes)
                {
                    if(reference.TryGetTarget(out var theme))
                    {
                        bool light = theme is LightTheme;
                        sb.Append(light ? "Light" : "Dark")
                            .AppendLine(" theme");
                    }

                }
                return sb.ToString();
            }
        }
    }
    public class ReplaceFactory
    {
        private readonly List<WeakReference< Ref<ITheme>>> themes =
            new List<WeakReference<Ref<ITheme>>>();
        public ITheme CreateThemeImp(bool light)
        {

            if (light)
                return new LightTheme();
            else
                return new DarkTheme();

        }
        public Ref<ITheme> CreateTheme(bool light)
        {
            var r = new Ref < ITheme >( CreateThemeImp(light));
            themes.Add(new WeakReference<Ref<ITheme>>(r));
            return r;
        }
        public void Replace(bool light)
        {
            foreach(var r in themes)
            {
                if (r.TryGetTarget(out var reference))
                {

                    reference.value = CreateThemeImp(light);
                }
            }
        }
    }

    public class Ref<T> where T : class
    {
        public T value { set; get; }
        public Ref(T value)
        {
            this.value = value;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ITheme theme1 = new DarkTheme();
            ITheme theme2 = new LightTheme();
            //Console.WriteLine(theme2);

            var factory = new TrackingThemeFactory();
            var theme3 = factory.CreateThemem(true);
            var theme4 = factory.CreateThemem(false);
            //Console.WriteLine(factory.Info);

            var factory2 = new ReplaceFactory();
            var theme5 = factory2.CreateTheme(true);
            var theme6 = factory2.CreateTheme(false);
            Console.WriteLine(theme5.value.BgrColor);

            factory2.Replace(false);

            Console.WriteLine(theme5.value.BgrColor);


        }
    }
}
