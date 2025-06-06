// === GLOBAL VARIABLES ===
VAR reputation = 0
VAR expected_base = ""
VAR expected_syrup = ""
VAR served_base = ""
VAR served_syrup = ""
VAR coffee_ready = false
VAR client = ""

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
    ~ reputation = reputation + 2
    #rep_change:5
    #money:8
    -> on_client_served
- else:
    { served_base == expected_base:
        Ну, почти. Но на будущее — без сиропов.
        ~ reputation = reputation + 0
        #rep_change:3
        #money:6
        -> on_client_served
    - else:
        Это точно не капучино. Печально.
        ~ reputation = reputation - 1
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
    ~ reputation = reputation + 1
    #rep_change:4
    #money:4
    -> on_client_served
- else:
    { served_base == expected_base:
        Неплохо, но чего-то лишнего ты плеснула.
        ~ reputation = reputation + 0
        #rep_change:1
        #money:4
        -> on_client_served
    - else:
        ...
        ~ reputation = reputation - 2
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
    ~ reputation = reputation + 2
    #rep_change:5
    #money:7
    -> on_client_served
- else:
    { served_base == expected_base:
        Я просила карамель… но и так сойдёт.
        ~ reputation = reputation - 1
        #rep_change:1
        #money:6
        -> on_client_served
    - else:
        Серьёзно?.. Даже не раф. Удачи, девочка.
        ~ reputation = reputation - 2
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
Ты открываешь кофейню. За окнами морось.
- Очередной день в моей любимой кофейне.
- Как приятно сидеть с кружечкой горячего кофе, когда на улице дождливо.
- Нужно сегодня хорошо постараться, чтобы позаботиться о моих котиках...
-> END

=== player_end_shift_1 ===
Какая жуткая буря налетела.
И что-за странный человек был за окном. Надеюсь, я его больше не увижу.
Пора закрывать кофейню и кормить котиков.
-> END

=== system_end_shift ===
#end_shift:1
-> END