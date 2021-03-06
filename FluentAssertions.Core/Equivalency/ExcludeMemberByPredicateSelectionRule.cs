﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Selection rule that removes a particular member from the structural comparison based on a predicate.
    /// </summary>
    internal class ExcludeMemberByPredicateSelectionRule : IMemberSelectionRule
    {
        private readonly Func<ISubjectInfo, bool> predicate;
        private readonly string description;

        public ExcludeMemberByPredicateSelectionRule(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            description = predicate.Body.ToString();
            this.predicate = predicate.Compile();
        }

        public IEnumerable<ISelectedMemberInfo> SelectMembers(IEnumerable<ISelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
        {
            return selectedMembers.Where(p => !predicate(new NestedSelectionContext(context, p))).ToArray();
        }

        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Exclude member when " + description;
        }
    }
}