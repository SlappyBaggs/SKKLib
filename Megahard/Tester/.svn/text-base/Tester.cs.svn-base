using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace Megahard.Tester
{

    public delegate void TestComponentHandler();

    public class TestEngine
    {
        private Queue<TestComponent> compQueue_ = new Queue<TestComponent>();
        private AsyncOperation asyncOp_ = AsyncOperationManager.CreateOperation(null);

        public event EventHandler<TestEventArgs> TestComplete;
        public event EventHandler<TestEventArgs> TestAborted;
        public event EventHandler<TestComponentEventArgs> TestUpdate;

//        private SendOrPostCallback testUpdateDelegate_;
//        private SendOrPostCallback testCompleteDelegate_;
       
        public TestEngine()
        {
            Abort_ = false;
        }

        public void AddComponent(TestComponent tc)
        {
            compQueue_.Enqueue(tc);
        }

        public void StartTest()
        {
            Abort_ = false;
            // See if we even have any components to test...
            if(compQueue_.Count == 0)
            {
                if(TestComplete != null)
                    TestComplete(this, new TestEventArgs(TestResult.CRITICAL_FAIL, "No test components added."));
                return;
            }

            Func<TestResults> startTestDel = StartTestAsync;
            startTestDel.BeginInvoke(ar =>
                {
                    var results = startTestDel.EndInvoke(ar);
                    if (results.Result == TestResult.ABORTED)
                    {
                        asyncOp_.PostOperationCompleted(
                            delegate
                            {
                                var testAborted_ = TestAborted;
                                if (testAborted_ != null)
                                    testAborted_(this, new TestEventArgs((TestResults)results));
                            },
                            null);
                    }
                    else
                    {
                        asyncOp_.PostOperationCompleted(
                            delegate
                            {
                                var testComplete_ = TestComplete;
                                if (testComplete_ != null)
                                    testComplete_(this, new TestEventArgs((TestResults)results));
                            },
                            null);
                    }
                }, null);
        }

        // This needs to pass back out a final TestResult...
        private TestResults StartTestAsync()
        {
            // We're running on a new thread... now loop through the components one at a time, doing their function...
            TestResults ret = new TestResults(TestResult.NONE, "No tests.");
            bool skip = false;
            foreach (TestComponent tc in compQueue_)
            {
                if (Abort_)
                {
                    // Test was aborted, so 
                    ret.Result = TestResult.ABORTED;
                    return ret;
                }

                if (skip)
                {
                    skip = false;
                    continue;
                }

                TestComponentEventArgs tcea = tc.StartComponent();

                if (tcea == null)
                    continue;
                
                // Post an update with the pass fail of that part of the test...
                asyncOp_.Post(
                    delegate
                    {
                        var testUpdate_ = TestUpdate;
                        if (testUpdate_ != null)
                            testUpdate_(this, tcea);
                    }, null);

                if (tcea.Results.Result == TestResult.PASS_END)
                {
                    // Bail out of everything here, we're done...
                    ret.Result = TestResult.PASS_END;
                    ret.passed_++;
                    return ret;
                }
                else if (tcea.Results.Result == TestResult.CRITICAL_FAIL)
                {
                    // Bail out of everything here, we're done...
                    ret.Result = TestResult.CRITICAL_FAIL;
                    ret.failed_++;
                    return ret;
                }
                else if (ret.Result == TestResult.NONE)  // If it's none, we haven't done any tests yet, so this is the first one...
                {
                    ret = tcea.Results;
                }
                else
                {
                    // We look if what we have is different, which means were not all pass or all fail, so we are partial...
                    if (ret.Result != tcea.Results.Result)
                        ret.Result = TestResult.PARTIAL;
                }

                if (tcea.Results.Result == TestResult.PASS)
                    ret.passed_++;
                else if (tcea.Results.Result == TestResult.PASS_SKIP)
                {
                    ret.passed_++;
                    ret.skipped_++;
                    skip = true;
                }
                else if (tcea.Results.Result == TestResult.FAIL)
                    ret.failed_++;
            }
        
            // Test complete without critical fail...
            return ret;
        }

        private bool Abort_;
        public void AbortTest()
        {
            // Manually end the test and process results...
            Abort_ = true;
        }
    }

    
    
    
    
    public abstract class TestComponent
    {
        private readonly string name_;
        public string Name { get { return name_; } }

        private readonly TestComponentHandler testHandler_;
        public TestComponent(string n)
        {
            name_ = n;
            testHandler_ = InternalTest;
            args_ = new TestComponentEventArgs(name_);
        }

        protected TestComponentEventArgs args_;
        internal TestComponentEventArgs StartComponent()
        {
            try
            {
                testHandler_();
            }
            catch (System.Exception e)
            {
                SetResultStatus(TestResult.CRITICAL_FAIL, "Exception caught:" + e.Message);// + ":" + e.InnerException.Message);
            }
            return args_;
        }

        protected void SetResultStatus(TestResult tr, string notes)
        {
            args_.Results.Result = tr;
            args_.Results.Notes = notes;
        }

        public abstract void InternalTest();
    }

    public class TestComponentWait : TestComponent
    {
        public TestComponentWait(int ms) : base("Wait")
        {
            wait_ = ms;
        }

        private int wait_;
        public override void InternalTest()
        {
            Thread.Sleep(wait_);
            SetResultStatus(TestResult.PASS, "Waited for " + wait_.ToString() + " ms.");
        }
    }

    
    public enum TestResult
    {
        NONE = 0,                   // No components have been tested yet, for internal use only...
        PASS = 1,                   // All components passed...
        PARTIAL = 2,                // Some components passed, some failed...
        FAIL = 3,                   // All components failed...
        CRITICAL_FAIL = 4,          // Something horrible went wrong, test was aborted prematurely...
        ABORTED = 5,                // Test was manually aborted...
        PASS_SKIP = 6,              // Test passed, but we want to skip the next step in the test...
        PASS_END = 7,               // Test ended prematurely, but for legit reasons...
    }

    public class TestResults
    {
        internal TestResults()
        {
            Result = TestResult.NONE;
            Notes = "";
            passed_ = failed_ = 0;
        }

        public TestResults(TestResult tr, string n)
        {
            Result = tr;
            Notes = n;
            passed_ = failed_ = skipped_ = 0;
        }

        public TestResult Result;
        public string Notes;
        public int passed_;
        public int failed_;
        public int skipped_;
    }

    public class TestEventArgs : EventArgs
    {
        internal TestEventArgs()
        {
        }
        
        public TestEventArgs(TestResults r)
        {
            Results = r;
        }

        public TestEventArgs(TestResult r, string s)
        {
            Results.Result = r;
            Results.Notes = s;
        }
    
        public TestResults Results;
    }

    public class TestComponentEventArgs : TestEventArgs
    {
        internal TestComponentEventArgs(string n)
        {
            Name = n;
            Results = new TestResults();
        }
        public TestComponentEventArgs(string n, TestResults r) : base(r)
        {
            Name = n;
            Results = r;
        }
        public TestComponentEventArgs(string n, TestResult r, string s) : base(r, s)
        {
            Name = n;
            Results = new TestResults(r, s);
        }
        
        public string Name;
    }
}
