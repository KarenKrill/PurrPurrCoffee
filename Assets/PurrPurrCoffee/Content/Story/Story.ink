// === GLOBAL VARIABLES ===
VAR star_raiting = 0
VAR paid_amount = 0
VAR expected_base = ""
VAR expected_syrup = ""
VAR served_base = ""
VAR served_syrup = ""
VAR coffee_ready = false
VAR client = ""
VAR playerName = "Alice"

=== coffee_machine ===
-> coffee_base_choice

=== coffee_base_choice ===
~ client = "Кофемашина"
Выберите кофе
+ [Кофе]
    ~ served_base = "water"
    -> coffee_syrup_choice
+ [Кофе с молоком]
    ~ served_base = "milk"
    -> coffee_syrup_choice

=== coffee_syrup_choice ===
~ client = "Кофемашина"
Выберите сироп
+ [Карамель]
    ~ served_syrup = "caramel"
    //~ coffee_ready = true
    -> END
+ [Клубника]
    ~ served_syrup = "strawberry"
    //~ coffee_ready = true
    -> END
+ [Без сиропа]
    ~ served_syrup = ""
    //~ coffee_ready = true
    -> END

// === CLIENT: МАРИЯ ===
=== client_maria ===
~ client = "Мария"
~ expected_base = "milk"
~ expected_syrup = ""
{ coffee_ready == true:
    -> maria_result
- else:
    Хочу капучино. Без сахара, без всего.
    -> END
}

=== maria_result ===
{ served_base == expected_base and served_syrup == expected_syrup:
    Отлично. Именно то, что нужно.
    ~ star_raiting = 5
    ~ paid_amount = 8
    #rep_change:5
    #money:8
    -> on_client_served
- else:
    { served_base == expected_base:
        Ну, почти. Но на будущее — без сиропов.
        ~ star_raiting = 3
        ~ paid_amount = 6
        #rep_change:3
        #money:6
        -> on_client_served
    - else:
        Это точно не капучино. Печально.
        ~ star_raiting = 1
        ~ paid_amount = 4
        #rep_change:1
        #money:4
        -> on_client_served
    }
}

// === CLIENT: ИГОРЬ ===
=== client_igor ===
~ client = "Игорь"
~ expected_base = "water"
~ expected_syrup = ""
{ coffee_ready == true:
    -> igor_result
- else:
    Эспрессо. Горький. Как моя жизнь.
    -> END
}

=== igor_result ===
{ served_base == expected_base and served_syrup == expected_syrup:
    В самый раз. Почувствуй бездну.
    ~ star_raiting = 4
    ~ paid_amount = 4
    #rep_change:4
    #money:4
    -> on_client_served
- else:
    { served_base == expected_base:
        Неплохо, но чего-то лишнего ты плеснула.
        ~ star_raiting = 1
        ~ paid_amount = 4
        #rep_change:1
        #money:4
        -> on_client_served
    - else:
        ...
        ~ star_raiting = 0
        ~ paid_amount = 4
        #rep_change:0
        #money:4
        -> on_client_served
    }
}

// === CLIENT: ОЛЬГА ===
=== client_olga ===
~ client = "Ольга"
~ expected_base = "milk"
~ expected_syrup = "caramel"
{ coffee_ready == true:
    -> olga_result
- else:
    Ммм... Я бы не отказалась от рафа с карамелью.
    -> END
}

=== olga_result ===
{ served_base == expected_base and served_syrup == expected_syrup:
    Вот это по мне. Спасибо!
    ~ star_raiting = 5
    ~ paid_amount = 7
    #rep_change:5
    #money:7
    -> on_client_served
- else:
    { served_base == expected_base:
        Я просила карамель… но и так сойдёт.
        ~ star_raiting = 1
        ~ paid_amount = 6
        #rep_change:1
        #money:6
        -> on_client_served
    - else:
        Серьёзно?.. Даже не раф. Удачи, девочка.
        ~ star_raiting = 0
        ~ paid_amount = 4
        #rep_change:0
        #money:4
        -> on_client_served
    }
}

=== on_client_served ===
~ coffee_ready = false
-> END

// === SYSTEM NODES (called from Unity) ===
=== player_shift_1_intro ===
- Где это я?
- О нет, я опять уснула на работе.
- Соберись, {playerName}, а то совсем всех клиентов растеряешь.
- До открытия кофейни всего пара минут, приборку придётся отложить.
//- Так, отлично, у меня есть ещё 10 минут до открытия. Нужно привести всё здесь в порядок.
-> END

=== player_end_shift_1 ===
Какая жуткая буря налетела.
И что-за странный человек был за окном. Надеюсь, я его больше не увижу.
Пора закрывать кофейню и кормить котиков.
-> END

=== system_end_shift ===
#end_shift:1
-> END