// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
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
    /// This file contains a class library that offers an implementation of a dependency graph. 
    /// This class also offers methods to be performed on said dependency graphs.
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on)
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// This dictionary represents the dependents within a DependencyGraph,
        /// where the keys are nodes and the associated values are HashSets of 
        /// all dependents of the associate nodes. 
        /// </summary>
        private Dictionary<string, HashSet<string>> dependents;


        /// <summary>
        /// This dictionary represents the dependees within a DependencyGraph,
        /// where the keys are nodes and the associated values are HashSets of 
        /// all dependees of the associate nodes. 
        /// </summary>
        private Dictionary<string, HashSet<string>> dependees;


        /// <summary>
        /// This variable will keep track of the size of a DependencyGraph, ie.
        /// the number of ordered pairs in the graph.
        /// </summary>
        private int size;


        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// Return the size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// </summary>
        /// <param name="s"> s represents the dependent to return the size of the dependees 
        /// of </param>
        /// <returns> the number of dependees s has </returns>
        public int this[string s]
        {
            get { if (!dependees.ContainsKey(s)) return 0; else return dependees[s].Count; }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        /// <param name="s"> s represents the dependee for which we are determining if it 
        /// has any dependents </param>
        /// <returns> true if s has dependents, false otherwise </returns>
        public bool HasDependents(string s)
        {
            if (!dependents.ContainsKey(s))
            {
                return false;
            }

            return dependents[s].Count != 0;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        /// <param name="s"> s represents the dependent for which we are determining if it 
        /// has any dependees </param>
        /// <returns> true if s has dependees, false otherwise </returns>
        public bool HasDependees(string s)
        {
            if (!dependees.ContainsKey(s))
            {
                return false;
            }

            return dependees[s].Count != 0;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        /// <param name="s"> s represents the dependee to enumerate the dependents of </param>
        /// <returns> returns an IEnumerable that can enumerate the dependents of s </returns>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                return dependents[s];
            }

            return new HashSet<string>();
        }


        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        /// <param name="s"> s represents the dependent to enumerate the dependees of </param>
        /// <returns> returns an IEnumerable that can enumerate the dependees of s </returns>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                return dependees[s];
            }

            return new HashSet<string>();
        }


        /// <summary>
        /// This is a helper method meant to be called in the AddDependency method after
        /// a change to the dependents dictionary. It will mirror said change in the 
        /// dependees dictionary accordingly.
        /// </summary>
        /// <param name="t"> t represents the dependent to add a dependee to </param>
        /// <param name="s"> s represents the dependee to be added </param>
        private void MirrorChange(string t, string s)
        {
            // check if we have to make a new entry for t in the dependees dictionary, 
            // or if we just have to add s to t's already existing list of dependees
            if (!dependees.ContainsKey(t))
            {
                dependees.Add(t, new HashSet<string>());
                dependees[t].Add(s);
            }

            // we now know that t is in the dependees dictionary, but we must add s to its
            // list of dependees
            else
            {
                dependees[t].Add(s);
            }
        }


        /// <summary>
        /// Adds the ordered pair (s,t), if it doesn't exist
        /// </summary>
        /// <param name="s"> s represents the dependee, s must be evaluated first </param>
        /// <param name="t"> t represents the dependent, t cannot be evaluated until s is </param>
        public void AddDependency(string s, string t)
        {

            // if s is not in the dependents dictionary, then you know the dependency
            // does not already exist
            if (!dependents.ContainsKey(s))
            {
                dependents.Add(s, new HashSet<string>());
                dependents[s].Add(t);
                size++;

                // mirror the change in the dependees dictionary as well
                MirrorChange(t, s);
            }

            // if s is already in the dependents dictionary
            else
            {
                HashSet<string> dependentList = dependents[s];

                // make sure that the dependency doesn't already exist
                if (!dependentList.Contains(t))
                {
                    dependentList.Add(t);
                    size++;

                    // mirror the change in the dependees dictionary as well
                    MirrorChange(t, s);
                }
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"> s represents the dependee </param>
        /// <param name="t"> t represents the dependent </param>
        public void RemoveDependency(string s, string t)
        {
            // first check if s is already in the dependents dictionary
            if (dependents.ContainsKey(s))
            {
                // then if the dependents removal is successful, decrement the size
                // and mirror the change in the dependees dictionary
                if (dependents[s].Remove(t))
                {
                    size--;
                    dependees[t].Remove(s);
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r). Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        /// <param name="s"> s represents the dependee to replace all dependents of </param>
        /// <param name="newDependents"> newDependents represents the list of new dependents
        /// to replace s's old dependents with </param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        { 
            if (dependents.ContainsKey(s))
            {
                // mirror the future change of clearing the dependents of s by updating
                // the dependees dictionary accordingly
                foreach (string oldDependent in dependents[s])
                {
                    dependees[oldDependent].Remove(s);
                    size--;
                }
                
                dependents[s] = new HashSet<string>();
            }

            // create an entry for s in dependents since it doesn't already exist
            else
            {
                dependents.Add(s, new HashSet<string>());
            }

            // add each new dependent and mirror the changes in dependees as well
            foreach (string newDependent in newDependents)
            {
                dependents[s].Add(newDependent);
                size++;

                if (!dependees.ContainsKey(newDependent))
                {
                    dependees.Add(newDependent, new HashSet<string>());
                }
                dependees[newDependent].Add(s);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        /// <param name="s"> s represents the dependent to replace all dependees of </param>
        /// <param name="newDependees"> newDependees represents the list of new dependees
        /// to replace s's old dependees with </param>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.ContainsKey(s))
            {
                // mirror the future change of clearing the dependees of s by updating
                // the dependents dictionary accordingly
                foreach (string oldDependee in dependees[s])
                {
                    dependents[oldDependee].Remove(s);
                    size--;
                }
                
                dependees[s] = new HashSet<string>();
            }

            // create an entry for s in dependees since it doesn't already exist
            else
            {
                dependees.Add(s, new HashSet<string>());
            }

            // add each new dependee and mirror the changes in dependents as well
            foreach (string newDependee in newDependees)
            {
                dependees[s].Add(newDependee);
                size++;

                if (!dependents.ContainsKey(newDependee))
                {
                    dependents.Add(newDependee, new HashSet<string>());
                }
                dependents[newDependee].Add(s);
            }
        }
    }
}
