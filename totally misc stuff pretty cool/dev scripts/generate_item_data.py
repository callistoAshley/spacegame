import os
import traceback

lines = [
    "{",
    "\"name\": \"hello!\",",
    "\"food\": false,",
    "\"armour\": false,",
    "\"weapon\": false,",
    "\"healAmount\": 0,",
    "\"defense\": 0,",
    "\"attackDamage\": 0,",
    "\"battleText\": null,",
    "\"script\": null,",
    "\"scriptArgs\": null,",
]

f = open(os.getcwd() + "\\item_data.json", "w")
f.writelines(lines)
f.close()