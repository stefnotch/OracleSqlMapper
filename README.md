# OracleSqlMapper
A little C# to Oracle (PL)SQL Mapper

[Synthetic data generation could be done with this](https://github.com/sdv-dev/SDGym)

# Unique, pseudo-random numbers

Called "pseudorandom permutation"

> One use-case I had that I would first generate some arbitrary table entries. (sequential IDs) And then, another table that would reference each of those entries exactly once depending on the relationship. (pseudorandom permutation for the foreign keys)
> Another use case is unique "name-middlename-lastname" combinations. Imagine having 1000 each, then that'd be 1000^3 = a lot of different combinations. Just picking the first N and shuffling them would be quite booooring, so we better be fancy.


Boils down to 1:1 remapping functions. **A PRNG with this property is called "full cycle" or "full period". For example, "full period xorshift"

e.g. `[00,01,10,11]` can be shifted by -1 to `[01,10,11,00]` . Or the number pairs can be swapped `[01,00,11,10]` Or, since it's a power of two, the bits can be flipped `[11,10,01,00]`. Or the range can be reversed `[11,10,01,00]`

- `length`: range goes from 0 to length-1
- `x`: current number



**Non power of 2**

Just pad it up to a power of 2. And, after doing the computations, throw x away if x is >= length.


**Relevant links**

- https://stackoverflow.com/questions/61411498/why-does-rand-repeat-numbers-far-more-often-on-linux-than-mac
- https://en.wikipedia.org/wiki/Linear_congruential_generator
- https://en.wikipedia.org/wiki/Linear-feedback_shift_register
- https://en.wikipedia.org/wiki/Full_cycle
- http://datagenetics.com/blog/november12017/index.html
- https://users.ece.cmu.edu/~koopman/lfsr/index.html
- https://stackoverflow.com/questions/3583697/generating-full-period-full-cycle-random-numbers-or-permutations-similar-to-lcg
- https://stackoverflow.com/questions/3572095/prng-with-adjustable-period/3575618#3575618

**Generating all permutations**

1 million elements have 1.000.000! permutations. Which is `8.26393 × 10^5565708`

If we're smart, we might be able to run a super fast version of heap's algorithm for each element <https://en.wikipedia.org/wiki/Heap%27s_algorithm>. 

For simplicity, I'd recommend doing it on binary numbers!



**Proving that x can end up anywhere**

This proof has to take into account that the likelyhood of ending up somewhere has to be equal. It also has to make sure that it's independent of anything else.

Like, a shift-n can move x anywhere, with an equal likelyhood. But, it's not really independent...

## Shift-n

Shifts a range by n.

```
shift(x, length, n) {
  return (x + n) % length;
}
```

length: any

n: any

## Swap-2

Swaps the two adjacent elements.

```
swap2(x, length) {
  if(length is even) {
    return (x is even)? x + 1: x - 1;
  } else {
  	if(x == (length - 1)) {
  	  return x;
  	} else {
  	  return (x is even)? x + 1: x - 1;
  	}
  }
}
```

length: any, when it is odd, the last element is left in place



## 1 Element :heavy_check_mark:

`[0]` &rarr; `[0]`

## 2 Elements :heavy_check_mark:

`[0,1]` &rarr; `[0,1]`, `[1,0]`

- Random shift
- Swap/no swap

## 3 Elements

`[0,1,2]` &rarr; `[0,1,2]`, `[0,2,1]`, `[1,0,2]`, `[1,2,0]`, `[2,0,1]`, `[2,1,0]`



What's the best way to list all different combinations?
