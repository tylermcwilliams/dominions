# SUMMARY

Cultus (Latin: *cultivation, culture*) is a mod for Vintage Story which purpose is to add ingame mechanics involving societies and cultures.
This is done by adding non-playing characters (NPCs) as [Entities](https://apidocs.vintagestory.at/api/Vintagestory.API.Common.EntityAgent.html) that simulate individual people, who perform labor and have personal needs. To fullfill the purpose of the NPCs, the mod adds a layer of interaction between these Entities and the ingame World via Factions, Jobs and Duties.

***Please keep in mind: Certain elements being described may not have an implementation yet!***

# The NPCs

The existence of the NPC revolves around switching between specific activities based on both internal states, like hunger or danger, and duties provided by a job assigned to it.

## Tasks

The base Vintage Story library includes an [EntityBehavior](https://apidocs.vintagestory.at/api/Vintagestory.API.Common.Entities.EntityBehavior.html) implementation named [BehaviorTaskAi](https://github.com/anegostudios/vsessentialsmod/blob/master/Entity/AI/Task/BehaviorTaskAI.cs) which NPCs use to know if they should be performing Labor or taking care of their Vitals(e.g. eating and sleeping). 

## Jobs, and Duties

Labor routines are handled by an AiTask implementation named [AiTaskWork](https://github.com/tylermcwilliams/dominions/blob/master/mods/cultus/src/NPC/Tasks/AiTaskWork.cs).
AiTaskWork takes care of leading the NPC to where it needs in order to perform a current work duty, as well as performing said duty once in range. 

[Duties](https://github.com/tylermcwilliams/dominions/blob/master/mods/cultus/src/Work/Duties/IDuty.cs) small single-purpose errands. They often have other Duties nested within
their implementation that act as pre-requisites. For instance, A Duty to Till soil will have a nested Duty to find a Hoe needed to carry out the job, as long as the NPC lacks
a hoe, Running the Tilling duty will actually run the duty to find a Hoe; if the NPC loses the Hoe after having found it, the Duty to search for a Hoe will become active again.

Duties are created in order to split up the required labor needed to be fullfilled by a [Job](https://github.com/tylermcwilliams/dominions/tree/master/mods/cultus/src/Work/Jobs).
A Job acts as a ledger for a specific operation, which may include a physical workplace, and takes care of Duty creation and provision as well as tracking progress toward a production goal.


## (WIP) Vitals/Restoration

NPCs are meant to simulate real people, therefore they should have basic needs such as eating and sleeping. Like work, vitality should be handled by an AiTask implementation. Even though tending to Vitals will share a lot of functionality with how Work is handled, it would be preferrable to have seperate AiTasks so as not to generalize concern too much.

# Block Areas

Some Jobs require areas on which to operate for production. These are currently managed by blocks called AreaMarkers which are an obsolete way of to store and retrieve areas. 

# Factions

In order to lay out a layer of interaction between Players, NPCs and Work, Factions will be added.



