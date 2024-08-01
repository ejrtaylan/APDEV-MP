-> Ruffian
-> RuffianPhase2
-> RuffianPhase3
-> RuffianPhase2HALF


=== Ruffian ===
What do you want?
    + [See Loftwin recently?]
        -> RuffianPhase2
    + [Have you seen anything suspicious at night?]
        -> RuffianPhase2HALF


-> END

== RuffianPhase2HALF ==
Loftwin's up to something that's for sure, heard fights have broken out more often since he got back, not to mention the strange sounds coming from his manor.
    + [Strange sounds?]
        -> RuffianPhase3
    + [So you think he's up to something...]
        -> RuffianPhase3
-> END 


== RuffianPhase2 ==
Loftwin's been acting real shady. I saw some strange lights over at Pearlblood Manor at night. 
    + [Something's definitely up.]
        -> RuffianPhase3
    + [Thanks for the tip.]
        -> RuffianPhase3
    
-> END

== RuffianPhase3 ==
If you're trying to do something about it, you should head inside, the people in that there tavern are blabbermouths.
-> END
