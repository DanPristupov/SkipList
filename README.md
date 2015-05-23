Skip List
=================

A C# implementation of the Skip List data structure.

Skip list is a data structure which represents a set or a key-value collection. Search, Insert and Remove are performed in O(Log n) time. If you are interested in more details, read Wiki (https://en.wikipedia.org/wiki/Skip_list).

####Advantages:
- Quite good performance
- No need in balancing
- Simple algorithm
- Good memory usage (same or less than Red Black Tree).

####Some of disadvantages are as follows:
- Slightly worse performance comparing to Red Black Tree
- Possibility (though very small) that operation will take longer than usually (inheritance of the randomized approach).

These are the key features. There are also another ones of course. If you are interested, read the wiki article.


###Why?
So, if the performance is worse than RedBlack tree, why do we need skip list?

The skip list algorithm is **really** simple and can be used as a base for another more complex data structures, which are based on binary search. This simplicity allows to create a working prototype very quickly. For example skip list can be used as a base for the following data structures:

- Sorted Dictionary or Sorted Set
- Lock Dictionary
- Lock Free Dictionary (!)
- Interval Set (!)

Few performance tests (I used Bcl.Dictionary<T> as an opponent):

Search in SkipList is slower than in RBTree.

TODO

Insert is almost the same until 256,000 items. After that RBTree wins.

TODO

###Summary
SkipList is a handy data structure for prototyping algorithms based on binary search. It is very simple. Performance is not much worse than native .Net data structures.