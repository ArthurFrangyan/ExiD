using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Generator.Collections
{
    [SuppressMessage( "ReSharper", "InconsistentNaming" )]
    internal static class SR {
        internal const string ArgumentOutOfRange_NeedNonNegNum = "Non-negative number required.";
        internal const string ArgumentOutOfRange_IndexMustBeLessOrEqual = "Index must be less or equal";
        internal const string InvalidOperation_EmptyQueue = "The queue is empty.";
        internal const string InvalidOperation_EnumFailedVersion = "Collection modified while iterating over it.";
        internal const string Arg_NonZeroLowerBound = "Non-zero lower bound required.";
        internal const string Arg_RankMultiDimNotSupported = "Multi-dimensional arrays not supported.";
        internal const string Argument_InvalidArrayType = "Invalid array type.";
        internal const string Argument_InvalidOffLen = "Invalid offset or length.";
    }
    internal static class ArrayEx {
        internal const int MaxLength = int.MaxValue;
    }
    public class PriorityQueue<TElement, TPriority>
    {
        private struct Node
        {
            public readonly TElement Element;
            public readonly TPriority Priority;
            public Node(TElement element, TPriority priority)
            {
                Element = element;
                Priority = priority;
            }
        }
        
        private const int Arity = 4;
        private const int Log2Arity = 2;

        private Node[] _nodes;
        private int _size;
        private int _version;
        public int Count => _size;

        
        public PriorityQueue()
        {
            _nodes = Array.Empty<Node>();
        }
        public PriorityQueue( int initialCapacity ) 
        {
            if ( initialCapacity < 0 ) {
                throw new ArgumentOutOfRangeException(
                    nameof(initialCapacity), initialCapacity, SR.ArgumentOutOfRange_NeedNonNegNum );
            }

            _nodes = new Node[initialCapacity];
        }

        public void Remove(TElement element)
        {
            int index = -1;

            for (int i = 0; i < _size; i++)
            {
                if (EqualityComparer<TElement>.Default.Equals(_nodes[i].Element, element))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                return;

            _version++;
            int lastIndex = --_size;

            if (index == lastIndex)
            {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<Node>())
                    _nodes[index] = default;
                return;
            }

            Node lastNode = _nodes[lastIndex];
            _nodes[lastIndex] = default;

            MoveDownDefaultComparer(lastNode, index);

            if (EqualityComparer<TElement>.Default.Equals(_nodes[index].Element, lastNode.Element) == false)
                MoveUpDefaultComparer(_nodes[index], index);
        }
        public TElement Peek() 
        {
            if ( _size == 0 ) {
                throw new InvalidOperationException( SR.InvalidOperation_EmptyQueue );
            }

            return _nodes[0].Element;
        }

        public TElement Dequeue() 
        {
            if ( _size == 0 ) {
                throw new InvalidOperationException( SR.InvalidOperation_EmptyQueue );
            }

            TElement element = _nodes[0].Element;
            RemoveRootNode();
            return element;
        }

        public void Enqueue(TElement element, TPriority priority)
        {
            int currentSize = _size++;
            _version++;

            if ( _nodes.Length == currentSize ) {
                Grow( currentSize + 1 );
            } 
            MoveUpDefaultComparer(new Node(element, priority), currentSize );
        }

        private void Grow( int minCapacity ) 
        {
            const int GrowFactor = 2;
            const int MinimumGrow = 4;

            int newcapacity = GrowFactor * _nodes.Length;

            if ( (uint) newcapacity > ArrayEx.MaxLength ) {
                newcapacity = ArrayEx.MaxLength;
            }

            newcapacity = Math.Max( newcapacity, _nodes.Length + MinimumGrow );

            if ( newcapacity < minCapacity ) {
                newcapacity = minCapacity;
            }

            Array.Resize( ref _nodes, newcapacity );
        }
        private void RemoveRootNode() 
        {
            int lastNodeIndex = --_size;
            _version++;

            if ( lastNodeIndex > 0 ) {
                var lastNode = _nodes[lastNodeIndex];
                MoveDownDefaultComparer( lastNode, 0 );
            }

            if ( RuntimeHelpers.IsReferenceOrContainsReferences<Node>() ) {
                _nodes[lastNodeIndex] = default;
            }
        }

        private void Heapify() 
        {
            Node[] nodes = _nodes;
            int lastParentWithChildren = GetParentIndex( _size - 1 );

            for ( int index = lastParentWithChildren; index >= 0; --index ) {
                MoveDownDefaultComparer( nodes[index], index );
            }
        }
        private void MoveUpDefaultComparer( Node node, int nodeIndex ) 
        {
            Node[] nodes = _nodes;

            while ( nodeIndex > 0 ) {
                int parentIndex = GetParentIndex( nodeIndex );
                Node parent = nodes[parentIndex];

                if ( Comparer<TPriority>.Default.Compare( node.Priority, parent.Priority ) < 0 ) {
                    nodes[nodeIndex] = parent;
                    nodeIndex = parentIndex;
                } else {
                    break;
                }
            }

            nodes[nodeIndex] = node;
        }
        private void MoveDownDefaultComparer( Node node, int nodeIndex ) 
        {
            Node[] nodes = _nodes;
            int size = _size;

            int i;
            while ( ( i = GetFirstChildIndex(nodeIndex) ) < size ) {
                Node minChild = nodes[i];
                int minChildIndex = i;

                int childIndexUpperBound = Math.Min( i + Arity, size );
                while ( ++i < childIndexUpperBound ) {
                    Node nextChild = nodes[i];
                    if ( Comparer<TPriority>.Default.Compare( nextChild.Priority, minChild.Priority ) < 0 ) {
                        minChild = nextChild;
                        minChildIndex = i;
                    }
                }

                if ( Comparer<TPriority>.Default.Compare( node.Priority, minChild.Priority ) <= 0 ) {
                    break;
                }

                nodes[nodeIndex] = minChild;
                nodeIndex = minChildIndex;
            }

            nodes[nodeIndex] = node;
        }

        private static int GetParentIndex( int index ) => ( index - 1 ) >> Log2Arity;
        private static int GetFirstChildIndex( int index ) => ( index << Log2Arity ) + 1;
    }
}