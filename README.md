Mongooz.Scheduler
=================

Customisable blocking, non-asynchronous retry scheduler with a fluent interface.

```csharp
IExecute executor = new RetryExecutor().
  WithDelay(100).             //Initial delay of 100ms before retry
  WithExponentialBackoff(2).  //Exponential increase delay
  WithMaximumDelay(10000).    //Maximum delay of 10000ms
  WithJitter(100).            //Random delay increase of up to 100ms
  WithMaximumRetries(2);      //Maximum number of 2 retries
  
executor.Execute(dangerousOperation);
```
