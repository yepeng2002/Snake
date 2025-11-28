using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace Snake.Api.Test
{
    [TestClass]
    public class UnitTestBase
    {
        /// <summary>
        /// 在执行为执行选择的第一个 TestClass() 中的第一个 TestMethod() 之前，执行带有该属性的方法
        /// </summary>
        /// <param name="tc"></param>
        [AssemblyInitialize()]
        public static void MyAssemblyInitialize(TestContext tc) { }

        /// <summary>
        /// 在执行为执行选择的第一个 TestClass() 中的第一个 TestMethod() 之后，执行带有该属性的方法
        /// </summary>
        /// <param name="tc"></param>
        [AssemblyCleanup()]
        public static void MyAssemblyCleanup(TestContext tc) { }

        /// <summary>
        /// 在整个测试class运行开始时候运行。可以在这个方法中添加所有测试方法都需要初始化的初始化代码。
        /// </summary>
        /// <param name="testContext">包含了单元测试的基本信息</param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) { }

        /// <summary>
        /// 在这个测试class运行结束的时候运行，和上一个标签对应
        /// </summary>
        [ClassCleanup()]
        public static void MyClassCleanup() { }

        /// <summary>
        /// 在执行每个 TestMethod() 之前调用
        /// </summary>
        [TestInitialize()]
        public virtual void MyTestInitialize() { }

        /// <summary>
        /// 在执行每个 TestMethod() 之后调用
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup() { }

        /// <summary>
        /// 忽略 TestMethod() 或 TestClass()
        /// </summary>
        [Ignore()]
        [TestMethod()]
        public void MyTestIgnore() { }
    }
}
