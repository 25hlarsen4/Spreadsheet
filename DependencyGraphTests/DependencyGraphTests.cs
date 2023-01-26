using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    /// Author:      Hannah Larsen
    /// Partner:     None
    /// Date:        20-Jan-2023
    /// Course:      CS3500, University of Utah, School of Computing
    /// Copyright:   CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
    /// 
    /// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
    /// All references used in the completion of the assignment are cited in my README file.
    /// 
    /// File Contents:
    /// This file contains a test class for DependencyGraphTest and is intended to contain all DependencyGraphTest Unit Tests.
    /// It tests all methods in the DependencyGraph class.
    /// </summary>
    [TestClass()]
    public class DependencyGraphTest
    {

        /// <summary>
        /// This tests the indexer method in a basic case.
        /// </summary>
        [TestMethod()]
        public void IndexerTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.AreEqual(2, t["b"]);
        }


        /// <summary>
        /// This tests the indexer method in the case that the input
        /// has no dependees.
        /// </summary>
        [TestMethod()]
        public void NonexistentNodeIndexerTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.AreEqual(0, t["a"]);
        }


        /// <summary>
        /// This tests the HasDependents method in the case that
        /// it returns true.
        /// </summary>
        [TestMethod()]
        public void HasDependentsTrueTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.IsTrue(t.HasDependents("c"));
        }


        /// <summary>
        /// This tests the HasDependents method in the case that
        /// it returns false.
        /// </summary>
        [TestMethod()]
        public void HasDependentsFalseTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.IsFalse(t.HasDependents("b"));
        }


        /// <summary>
        /// This tests the HasDependents method in the case that it is 
        /// called with an input node that does not exists in the graph.
        /// </summary>
        [TestMethod()]
        public void NonexistentNodeHasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.IsFalse(t.HasDependents("p"));
        }


        /// <summary>
        /// This tests the HasDependees method in the case that
        /// it returns true.
        /// </summary>
        [TestMethod()]
        public void HasDependeesTrueTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.IsTrue(t.HasDependees("d"));
        }


        /// <summary>
        /// This tests the HasDependees method in the case that
        /// it returns false.
        /// </summary>
        [TestMethod()]
        public void HasDependeesFalseTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.IsFalse(t.HasDependees("a"));
        }


        /// <summary>
        /// This tests the HasDependees method in the case that it is 
        /// called with an input node that does not exists in the graph.
        /// </summary>
        [TestMethod()]
        public void NonexistentNodeHasDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("d", "b");
            Assert.IsFalse(t.HasDependents("p"));
        }


        /// <summary>
        /// This tests the Size method on an empty graph.
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        /// This tests the AddDependency method in a basic case.
        /// </summary>
        [TestMethod()]
        public void AddDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);

            // check that the dependents and dependees were updated accordingly
            Assert.IsTrue(t.GetDependents("a").Contains("b"));
            Assert.IsTrue(t.GetDependees("b").Contains("a"));
        }


        /// <summary>
        /// This tests that you can add a cycle without error.
        /// </summary>
        [TestMethod()]
        public void AddDependencyCycleTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
            t.AddDependency("a", "a");
            Assert.AreEqual(1, t.Size);

            // check that the dependents and dependees were updated accordingly
            Assert.IsTrue(t.GetDependents("a").Contains("a"));
            Assert.IsTrue(t.GetDependees("a").Contains("a"));
        }


        /// <summary>
        /// This tests that you cannot add duplicate dependencies.
        /// </summary>
        [TestMethod()]
        public void AddDuplicateDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);

            // check that the dependents of a did not change
            IEnumerator<string> e1 = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("b", e1.Current);
            Assert.IsFalse(e1.MoveNext());

            // check that the dependees of b did not change
            IEnumerator<string> e2 = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("a", e2.Current);
            Assert.IsFalse(e2.MoveNext());
        }


        /// <summary>
        /// This tests the RemoveDependency method in a basic case.
        /// </summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        /// This tests the RemoveDependency method in the case that
        /// a dependency is still left within the graph.
        /// </summary>
        [TestMethod()]
        public void RemoveDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("a", "b");
            Assert.AreEqual(2, t.Size);
            t.RemoveDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }


        /// <summary>
        /// This tests that you can remove a cycle dependency
        /// without error.
        /// </summary>
        [TestMethod()]
        public void RemoveDependencyCycleTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "a");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("a", "a");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        /// This tests that nothing happens when you try to remove
        /// a dependency that doesn't exist.
        /// </summary>
        [TestMethod()]
        public void RemoveNonexistentDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("a", "b");
            Assert.AreEqual(2, t.Size);
            t.RemoveDependency("b", "a");
            Assert.AreEqual(2, t.Size);
        }


        /// <summary>
        /// The tests the GetDependents and GetDependees methods.
        /// </summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");

            // check that the pair was removed correctly
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        /// This tests that replacing on an empty DG should not fail.
        /// </summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }


        /// <summary>
        /// This tests that it is possible to have more than one DG at a time.
        /// </summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }


        /// <summary>
        /// This tests the size method.
        /// </summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        /// This tests that nodes' dependents and dependees are
        /// stored correctly.
        /// </summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            // check that a has no dependees
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            // check that b's dependees are a and c
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            // check that c's dependee is a
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            // check that d's dependee is b
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }


        /// <summary>
        /// This tests the ReplaceDependents and ReplaceDependees
        /// methods in a basic case.
        /// </summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            // check that a has no dependees
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            // check that b's dependees are a and c
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            // check that c's dependee is a
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            // check that d's dependee is b
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }


        /// <summary>
        /// This tests the ReplaceDependents method in the case that 
        /// the size of the DG grows as a result.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);

            // replace a's dependents with more than there were to begin with
            HashSet<string> set = new HashSet<String>() { "c", "s" };
            t.ReplaceDependents("a", set);
            Assert.AreEqual(2, t.Size);

            // check that a's dependents are correctly updated
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "c") && (s2 == "s")) || ((s1 == "s") && (s2 == "c")));

            // check that the dependees were updated accordingly
            Assert.IsTrue(t.GetDependees("c").Contains("a"));
            Assert.IsTrue(t.GetDependees("s").Contains("a"));
        }


        /// <summary>
        /// This tests the ReplaceDependents method in the case that the
        /// dependents are replaced with nothing.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsWithEmptySetTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "z");
            t.AddDependency("a", "d");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);

            // replace a's dependents with nothing
            HashSet<string> set = new HashSet<String>();
            t.ReplaceDependents("a", set);
            Assert.AreEqual(1, t.Size);

            // check that a has no dependents
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            // check that the dependees were updated accordingly
            Assert.IsFalse(t.GetDependees("b").Contains("a"));
            Assert.IsFalse(t.GetDependees("z").Contains("a"));
            Assert.IsFalse(t.GetDependees("d").Contains("a"));
        }


        /// <summary>
        /// This tests the ReplaceDependents method in the case that 
        /// the dependee is not originally in the DG.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsDependeeNotInGraphTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
            HashSet<string> set = new HashSet<string>() { "c", "s" };
            t.ReplaceDependents("a", set);
            Assert.AreEqual(2, t.Size);

            // check that a was added as a dependee and its dependents are c and s
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "c") && (s2 == "s")) || ((s1 == "s") && (s2 == "c")));

            // check that the dependees were updated accordingly
            Assert.IsTrue(t.GetDependees("c").Contains("a"));
            Assert.IsTrue(t.GetDependees("s").Contains("a"));
        }


        /// <summary>
        /// This tests the ReplaceDependees method in the case that 
        /// the size of the DG shrinks as a result.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "z");
            t.AddDependency("c", "b");
            Assert.AreEqual(3, t.Size);

            // replace b's dependees with less than there were to begin with
            HashSet<string> set = new HashSet<String>() { "p" };
            t.ReplaceDependees("b", set);
            Assert.AreEqual(2, t.Size);

            // check that b's only dependee is now p
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "p");

            // check that the dependents were updated accordingly
            Assert.IsTrue(t.GetDependents("p").Contains("b"));
        }


        /// <summary>
        /// This tests the ReplaceDependees method in the case that 
        /// the dependent is not originally in the DG.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependeesDependentNotInGraphTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
            HashSet<string> set = new HashSet<string>() { "c", "s" };
            t.ReplaceDependees("a", set);
            Assert.AreEqual(2, t.Size);

            // check that a was added as a dependent and its dependees are c and s
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "c") && (s2 == "s")) || ((s1 == "s") && (s2 == "c")));

            // check that the dependents were updated accordingly
            Assert.IsTrue(t.GetDependents("c").Contains("a"));
            Assert.IsTrue(t.GetDependents("s").Contains("a"));
        }


        /// <summary>
        /// This tests that you can use lots of data.
        /// </summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

    }
}
