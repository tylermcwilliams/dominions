# SUMMARY

Cultus (Latin: *cultivation, culture*) is a mod for Vintage Story which purpose is to add ingame mechanics involving societies and cultures.
This is done by adding non-playing characters (NPCs) as [Entities](https://apidocs.vintagestory.at/api/Vintagestory.API.Common.EntityAgent.html) that simulate individual people, who perform labor and have personal needs. To fullfill the purpose of the NPCs, the mod adds a layer of interaction between these Entities and the ingame World via Jobs and Duties, as well as a layer between the Players and the Entities via Factions(may change).

***Please keep in mind: Certain elements being described may not have an implementation yet!***

## The NPCs are Finite State Machines

##### Tasks, Jobs, and Duties

The base Vintage Story library includes an [EntityBehavior](https://apidocs.vintagestory.at/api/Vintagestory.API.Common.Entities.EntityBehavior.html) implementation named [BehaviorTaskAi](https://github.com/anegostudios/vsessentialsmod/blob/master/Entity/AI/Task/BehaviorTaskAI.cs) which NPCs use to know if they should be performing Labor or taking care of their Vitals(e.g. eating and sleeping). 

Labor routines are handled by a Task implementation named [AiTaskWork](https://github.com/tylermcwilliams/dominions/blob/master/mods/cultus/src/NPC/Tasks/AiTaskWork.cs).
AiTaskWork takes care of leading the NPC to where it needs in order to perform a current work duty, as well as performing said duty once in range. 

[Duties](https://github.com/tylermcwilliams/dominions/blob/master/mods/cultus/src/Work/Duties/IDuty.cs) small single-purpose errands. They often have other Duties nested within
their implementation that act as pre-requisites. For instance, A Duty to Till soil will have a nested Duty to find a Hoe needed to carry out the job, as long as the NPC lacks
a hoe, Running the Tilling duty will actually run the duty to find a Hoe; if the NPC loses the Hoe after having found it, the Duty to search for a Hoe will become active again.

Duties are created in order to split up the required labor needed to be fullfilled by a [Job](https://github.com/tylermcwilliams/dominions/tree/master/mods/cultus/src/Work/Jobs).
A Job acts as a ledger for a specific operation, which may include a physical workplace, and takes care of Duty creation and provision as well as tracking progress toward a production goal.


