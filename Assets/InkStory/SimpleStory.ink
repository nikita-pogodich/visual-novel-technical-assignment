VAR c1_choice = 0
VAR c2_choice = 0
VAR talked_to_c1 = false
VAR talked_to_c2 = false
VAR first_speaker = ""   // "c1" or "c2"

-> hub_quiet

=== hub_quiet ===
# mode:character_select
# bg:hub
# speaker:system
+ [Talk to Character 1]
    -> char1_root
+ [Talk to Character 2]
    -> char2_root

=== char1_root ===
# mode:in_conversation
# bg:c1_room
# speaker:char1
{ talked_to_c1:
    Character 1: We already talked.
    -> hub_quiet
- else:
    ~ talked_to_c1 = true
    { first_speaker == "":
        ~ first_speaker = "c1"
    }

    Character 1: Hey. I’ve been expecting you.
    Character 1: Before we continue, pick one option.

    + [Choice 1]
        ~ c1_choice = 1
        -> after_conversation
    + [Choice 2]
        ~ c1_choice = 2
        -> after_conversation
}

=== char2_root ===
# mode:in_conversation
# bg:c2_street
# speaker:char2
{ talked_to_c2:
    Character 2: We already talked.
    -> hub_quiet
- else:
    ~ talked_to_c2 = true
    { first_speaker == "":
        ~ first_speaker = "c2"
    }

    Character 2: You made it. Good.
    Character 2: Now choose carefully.

    + [Choice 1]
        ~ c2_choice = 1
        -> after_conversation
    + [Choice 2]
        ~ c2_choice = 2
        -> after_conversation
}

=== after_conversation ===
{ talked_to_c1 && talked_to_c2:
    -> evaluate_ending
- else:
    -> hub_quiet
}

=== evaluate_ending ===
{ first_speaker == "c1" && c1_choice == 2 && c2_choice == 1:
    -> good_ending
- else:
    -> bad_ending
}

=== good_ending ===
# bg:ending_good
# mode:ending
# ending:good
# speaker:system
GOOD ENDING: You did (C1: choice 2) first, then (C2: choice 1).
-> END

=== bad_ending ===
# bg:ending_bad
# mode:ending
# ending:bad
# speaker:system
BAD ENDING: Any other sequence/choices fails.
-> END
