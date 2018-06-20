# Magazine Store programming challenge

## Objective
This is part of the Magazine Store programming challenge. The objective is to write a C# console application which can reach out to a web API and gather information consisting of Magazine Categories, Magazines within those Categories, and Subscribers of those Magazines. Taking this data, we need to process it to determine which Subscribers are subscribed to a magazine from every category. Taking these Subscriber IDs, we then submit this to an endpoint to verify and return a result of success or failure and how long it took, printing this to the console.

The challenge purposely throttles certain operations to take anywhere from 1 to 4 seconds, so for extra credit the objective is to attempt to return your results in under 10 seconds. In order to achieve this, I opted to gather as much data immediately as possible, namely the categories and subscribers. Using this, I can then grab all magazines by category and sort them locally. Alternatively, a slower approach would have been to get the categories and subscribers, then check the API against each subscriber, making multiple calls. This would slow things down tremendously. Another approach I would have preferred to have taken would be to thread the calls so it could make the magazine calls for each category simultaneously, but for the sake of time I landed here.

## Running the program
This is a console application, you simply run it by name from the console or from Explorer. It will first retrieve an authentication token and print that on success, then retrieve the assorted information (categories, subscribers, magazines). Once finished it will print the result and time it took to run, prompting to press any key when ready to quit.
