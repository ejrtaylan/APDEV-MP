-> Guard
-> GuardPhase2
-> GuardPhase3
-> GuardPhase2HALF

=== Guard ===
How can I help you?
    + [I'm looking for Loftwin Pearlblood. Have you seen him around?]
        ->GuardPhase2
    + [So... anything happen lately?]
        ->GuardPhase2HALF


-> END

== GuardPhase2HALF ==
If you're looking into the mayor you could just tell me. He's been here recently, that's all I know anyway.
    +[Anyone know more?]
        ->GuardPhase3
    +[Where can I get more to this story?]
        ->GuardPhase3
->END 


== GuardPhase2 ==
The mayor visited our store a while back, acting all secretive. If you want to know more, you should talk to Marcus.
    + [Sure, I'll try.]
        ->GuardPhase3
    + [Thanks for the tip.]
        ->GuardPhase3
    
-> END

== GuardPhase3 ==
You should head inside, well, if you can pry Marcus for that information anyway. The two are close, so I doubt he'd be willing to sell out his friend without getting something in return.
-> END


    

