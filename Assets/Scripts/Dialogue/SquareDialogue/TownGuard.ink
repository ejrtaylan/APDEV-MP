-> Guard
-> GuardPhase2
-> GuardPhase3
-> GuardPhase2HALF

=== Guard ===
What brings you here?
    + [Have you noticed anything unusual?]
        ->GuardPhase2
    + [You look tired, what's wrong?]
        ->GuardPhase2HALF


-> END

== GuardPhase2HALF ==
Tell me about it, ever since the mayor got back from an expedition, the town's been in a little frenzy, people taking their own little issues into blown out brawls is starting to wear me out.
    +[You think the mayor's got something to do with this frenzy?]
        ->GuardPhase3
    +[Curious, so the mayor must have brought something to town.]
        ->GuardPhase3
->END 


== GuardPhase2 ==
Unusual is putting it lightly, even I've been feeling a little angry lately, but life's been alright so far, but I can't seem to take control of my anger recently, after the mayor's return
    + [That's not good.]
        ->GuardPhase3
    + [So conflict's been a problem in this town as of recently..]
        ->GuardPhase3
    
-> END

== GuardPhase3 ==
And it's getting worse. If anyone could figure out why this is happening, they'd be doing us a favor.
-> END