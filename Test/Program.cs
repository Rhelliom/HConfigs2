using HConfigs;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConfig config = new TestConfig
            {
                IntProperty = 5,
                StringProperty = "Potato",
                LongProperty = 54583838398078309,
                FloatProperty = 0.556f,
                DoubleProperty = 0.00066007,
                BoolProperty = true,
                StringWithComment = "Somewhere over the rainbow",
                RegionField1 = "Boop",
                RegionField2 = "Bop",
                RegionField3 = "Bedoop",
                UnmarkedProperty = "???"
            };

            Config.Save(config);

            TestConfig loaded = Config.Load<TestConfig>();

            Check(config.IntProperty, loaded.IntProperty);
            Check(config.StringProperty, loaded.StringProperty);
            Check(config.LongProperty, loaded.LongProperty);
            Check(config.FloatProperty, loaded.FloatProperty);
            Check(config.DoubleProperty, loaded.DoubleProperty);
            Check(config.BoolProperty, loaded.BoolProperty);
            Check(config.StringWithComment, loaded.StringWithComment);
            Check(config.RegionField1, loaded.RegionField1);
            Check(config.RegionField2, loaded.RegionField2);
            Check(config.RegionField3, loaded.RegionField3);
            Check(config.UnmarkedProperty, loaded.UnmarkedProperty);
        }

        static void Check(object a, object b)
        {
            if (a.Equals(b))
            {
                Console.Out.WriteLine("Pass");
            }
            else
            {
                Console.Out.WriteLine($"Fail: {a} / {b}");
            }
        }
    }
}
