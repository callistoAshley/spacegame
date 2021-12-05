import os
import traceback

lines = [
    "{\n",
    "\"name\": \"hello!\",\n",
    "\"description\": \"i am a cool item\",\n",
    "\"consumeAfterUse\": false,\n",
    "\"armour\": false,\n",
    "\"weapon\": false,\n",
    "\"healAmount\": 0,\n",
    "\"defense\": 0,\n",
    "\"attackDamage\": 0,\n",
    "\"battleText\": null,\n",
    "\"script\": null,\n",
    "\"scriptArgs\": null,\n",
    "\"shopPrice\": 0,\n",
    "\"canBeUsedInMenu\": false,\n",
    "\"canBeUsedInBattle\": false,\n",
    "}"
]

f = open(os.getcwd() + "\\item_data.json", "w")
f.writelines(lines)
f.close()