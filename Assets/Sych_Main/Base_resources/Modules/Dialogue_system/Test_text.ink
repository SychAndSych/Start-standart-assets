INCLUDE Globals_variables.ink

VAR Insta = false

VAR Answer = false

{Start_test_story == false: -> Main | -> Main_again}

=== Main ===
#Emotion:Talking #Speaker_name:Волчик

{Insta == false : Привет. Как дела? | Что-то ещё?}
~ Insta = true


{Answer == false :
* [Привет хорошо.]
Это <color=\#1CEC0E00>хорошо</color></b>.)

~ Answer = true
-> Main

* [Всё плохо.]
   Это <color=\#5B81FF>плохо</color></b>.(
   ~ Answer = true
   -> Main

+ [Не скажу.]
  Бяка.
-> Main
}

+ [Я пойду.]
~ Start_test_story = true
 Да, иди. #Emotion:Cry
 -> END 
 
 === Main_again === 
Я занят, всего хорошего!
-> END

 === Test_changed_id_dialogue === 
 + [Я пойду.]
 Да, иди. #Emotion:Cry #Change_id_dialogue:1
  -> END 
