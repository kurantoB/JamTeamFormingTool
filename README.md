# Jam Team Forming Tool (JTFT)

## What is this

'Tis but a simple groupwork match-making tool, borne of the hectic chaos of the game jam team-forming process.

It works, how? Say you have a bunch of people of a diverse assortment of skills and talents to lend to jam projects. You have, also, a bunch of teams in want of specific roles. Keyword: Bunch. The tool works best if you have a good-sized pool of people willing to partake, who would otherwise flood the channels with portfolios and team pitches - I'm talking specifically about the case where backlogging is time-consuming and choice-paralysis-ing.

With this premise, given these prancing grounds, it would do well to, dare I say, let loose an algorithm.

So yeah, this tool takes people's needs and capacities into account, so the options for welcoming teams / aspiring team inductees can be more precisely determined from the perspective of each party.

Wait, there's more.

There's no guarantee that every team seeker can find a team, and given that a good many participants have skills to contribute (usually, more numerous than there are open roles), it would be remiss to see these diverse skills go to waste.

The good news is that projects can be categorized into a set number of types. Each type of project requires its own collection of required skills and capacities. Wouldn't it be nice if a participant with skills to contribute can see which project "type" they are most fit for, and which participant (out of the many) would best be paired with them to fulfill the requirements of this project type? Wow, that was a mouthful. Anyway, yes. The answer is yes. And we can do that here, also.

## What is this really

This is an **ASP.NET Core** project. Data storage is done using **SQLite** - you can access the file from `JamTeamFormingTool/Data/jamteamformingtool.db`. The user interface should be pretty self-explanatory, whether you are a jam host or a participant.

## Would it hurt to actually show me what this is

Maybe a bit, but I'll do it all the same.

Everything shown below is a sample.

<hr/>

To use this tool, first you need a **team forming session**. (They represent a definite window of time that team-forming takes place for a jam.)

![image](https://github.com/user-attachments/assets/cde2420b-7cc1-4bad-8112-16dedc8277da)

<hr/>

Let's take a step back because a team forming session needs to based off a **role template**. (A role template is a collection of **roles** that are available for this team forming session, e.g. artist, composer, writer, etc.)

![image](https://github.com/user-attachments/assets/ec42de13-35bb-47c1-92a6-be3097462d29)

<hr/>

Role templates also contain information on project types, under a section known as **coverages**. Each type contains a list of **role categories**, which are themselves lists of roles that, if *any one of them in a given list* exist in a team, would provide coverage for that role category.

![image](https://github.com/user-attachments/assets/5cf03a23-5bab-4221-836f-b5453090a18a)

<hr/>

Ok, let's go back to team forming sessions. These animals have 2 sections: **registered teams**, and **registered team-seekers**. Teams are looking for people to fill roles, and team-seekers are looking for teams to take them in. Participants can join a session and register as either one of these.

![image](https://github.com/user-attachments/assets/9890cf87-79e0-43e4-bd21-ad23f4848ab5)

<hr/>

When sessions are first created, they are in the **registration** phase, so match-making isn't allowed yet. We don't want people to start matching when people are still registering. The pool is still immature. When the time is ripe (preferably pre-announced), the admin of the session can advance it to **team-forming** phase, during which match-making is allowed.

What does match-making look like? From a team's perspective, it's a ranking of team-seekers by how well their skills suit them.

![image](https://github.com/user-attachments/assets/5e61e046-989c-4b1f-8a73-0422dc3fb3c6)

And for a team-seeker, it's a ranking of teams by how well their own skills suit *them*, as well as a ranking of other team seekers + project type combos best for them.

![image](https://github.com/user-attachments/assets/71c64282-ee8c-4de5-ac9e-97f6e867efb3)

## What else is this

We've walked through the basic features so I hope you have a gist of what it does. But this tool is only half the equation. It only works best if it's shepherded with care. Many factors can lead to unwanted results such as
- Not enough awareness of JTFT within a specific jam community
- Small JTFT participation proportional to the actual pool size
- No clear announcement of team forming session registration opening or advancing to team-forming
- Missed schedules pertaining to the above, resulting in confusion
