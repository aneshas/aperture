# Aperture Projection Library

![Build and Test](https://github.com/aneshas/aperture/workflows/Build%20and%20Test/badge.svg)
![Push packages to NuGet.org](https://github.com/aneshas/aperture/workflows/Push%20packages%20to%20NuGet.org/badge.svg)

## Use with Event Sourcing

A thin wrapper around your projections (read model adapters) to better
handle offset tracking and most importantly to handle failures gracefully.

If you are using only CQRS without the event store you could still
use Aperture to make yourself easier.

Offers offset tracking and event stream adapters for :

- PostgreSQL
- MSSQL
- EventStore
- MediatR - TBD this might be useful? (provide event/command stream adapter for MediatR)

These adapters come pre built, but aperture is easily extensible
(adheres to ports and adapters style of architecture) and allows you
to create your own event stream/store, offset tracking, projection, failure strategy,
exception handling and logging adapters.

## Alternative use cases

Due to the nature of projections Aperture (being a simple adapters), they
could also be used for varieaty of other use cases such as, but not limited to:

- Implement outbox pattern
- Command sourcing (use command stream instead of event stream) - note that
  how they act as driving adapters (primary) here instead of driven (secondary).
  (TODO - Check primary secondary wording)

## Abstractions

Offset tracking adapters and Projection adapters are deliberately
segregated so you can be as flexible as you wan't.

You can for example use MSSQL adapters to both track your projection state (offset)
and use sql projection to ensure your offset and read model are saved in a transaction.

If you don't care about offset tracking you could use built-in no op offset
tracker (which always starts from 0) and still use sql projection adapter.

Or use any other combination of offset tracking and projection adapters  
(including your custom built ones).

TODO
List extendable adapters here and explain each (don't to forget to explain
actor model - like supervisors and the idea behind them)

Explain Core implementations

## Example project

## Aperture Web Dashboard

- TBD

## Guidelines for writting your own adapters

TODO - Explain each
