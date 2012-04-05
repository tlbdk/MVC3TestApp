using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;

namespace TestRunner
{
    class TestSuite
    {
        static void Main(string[] args)
        {
            int nTests = 0;
            int nFailedTests = 0;

            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.GetCustomAttributes(typeof(TestFixtureAttribute), false).Length != 0)
                {
                    // Gather test info.
                    MethodInfo mFixtureSetup = null;
                    MethodInfo mFixtureTearDown = null;
                    MethodInfo mSetup = null;
                    MethodInfo mTearDown = null;
                    List<MethodInfo> mTests = new List<MethodInfo>();
                    foreach (MethodInfo m in t.GetMethods())
                    {
                        if (m.GetCustomAttributes(typeof(TestFixtureSetUpAttribute), false).Length != 0)
                            mFixtureSetup = m;

                        if (m.GetCustomAttributes(typeof(TestFixtureTearDownAttribute), false).Length != 0)
                            mFixtureTearDown = m;

                        if (m.GetCustomAttributes(typeof(SetUpAttribute), false).Length != 0)
                            mSetup = m;

                        if (m.GetCustomAttributes(typeof(TearDownAttribute), false).Length != 0)
                            mTearDown = m;

                        if (m.GetCustomAttributes(typeof(TestAttribute), false).Length != 0)
                            mTests.Add(m);
                    }

                    // Run tests
                    object obj = Activator.CreateInstance(t);

                    if (mFixtureSetup != null)
                        mFixtureSetup.Invoke(obj, null);

                    foreach (MethodInfo m in mTests)
                    {
                        nTests++;
                        if (mSetup != null)
                            mSetup.Invoke(obj, null);

                        try
                        {
                            m.Invoke(obj, null);
                        }
                        catch (Exception)
                        {
                            nFailedTests++;
                            Console.WriteLine(String.Format("Exception thrown in {0} during {1} test. ", t.Name, m.Name));
                        }

                        if (mTearDown != null)
                            mTearDown.Invoke(obj, null);
                    }

                    if (mFixtureTearDown != null)
                        mFixtureTearDown.Invoke(obj, null);
                }
            }

            Console.WriteLine("{0} tests. {1} failed", nTests, nFailedTests, "Unit test results");
        }

    }
}
