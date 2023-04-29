# Aperture Projection Library

![Build and Test](https://github.com/aneshas/aperture/workflows/Build%20and%20Test/badge.svg)
![Push packages to NuGet.org](https://github.com/aneshas/aperture/workflows/Push%20packages%20to%20NuGet.org/badge.svg)

## What is it?
A blog post with a link to an example project [here](https://medium.com/@aneshas/c-event-sourcing-example-with-tactical-ddd-and-aperture-4ade39cbaac3)

A set of abstractions allowing you to implement event sourcing projections
with offset tracking and most importantly a way to handle failures gracefully.

If you are using only CQRS without the event store you could still
use Aperture to make your life easier.

Currently offset tracking and event stream adapters are implemented for:

- PostgreSQL

To come:
- MSSQL
- EventStore

These adapters come pre built (some in core some in adapter packages), 
but aperture is easily extensible (adheres to ports and adapters style of architecture) and allows you
to create your own event stream/store, offset tracking, projection, failure strategy,
exception handling and logging adapters.

## Abstractions

Offset tracking adapters and Projection adapters are deliberately
segregated so you can be as flexible as you wan't.

You can for example use Postgres adapters to both track your projection state (offset)
and use sql projection to ensure your offset and read model are saved in a transaction.

If you don't care about offset tracking you could use built-in no op offset
tracker (which always starts from 0) and still use sql projection adapter.

Or use any other combination of offset tracking and projection adapters  
(including your custom built ones).

## Other hings to come
- Bulk projections (smth like buffered streams with Flush)
- Ability to define retriable exceptions instead of retrying all ProjectionExceptions
- LRU for pull event stream?
- Expose API or Web Dashboard
