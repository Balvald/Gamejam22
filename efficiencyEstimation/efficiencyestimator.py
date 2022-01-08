import json
import math

moneyLocations = list()
coalLocations = list()
ironLocations = list()

gn250data = list()

with open('GN250.csv', "r", encoding="iso-8859-1") as file:
    for line in file:
        segment = line.split(";")
        #print(segment[3] + segment[16] + segment[17])
        if segment[0] == "NNID":
            continue
        # Translate to Game coordinates
        # posX
        segment[16] = (float(segment[16].strip("\n")) * 0.01) - 3951
        # posY
        segment[17] = (float(segment[17].strip("\n")) * 0.01) - 52714
        if segment[2] == "AX_Gemeinde":
            moneyLocations.append(segment)
        if segment[2] == "AX_Wald":
            coalLocations.append(segment)
        if segment[2] == "AX_TagebauGrubeSteinbruch":
            ironLocations.append(segment)
        if segment[2] == "AX_BauwerkOderAnlageFuerIndustrieUndGewerbe":
            ironLocations.append(segment)
        #gn250data.append(segment)

#print(ironLocations)

mainFile = open("main.json", "r", encoding="UTF-8")
json_object = json.load(mainFile)
mainFile.close()

for station in json_object["allStationData"]:

    amountMoney = 0
    amountCoal = 0
    amountIron = 0
    
    print(station["stationName"])
    
    posX = station["posX"]
    posY = station["posY"]

    #check for moneylocations nearby
    print("money")
    for location in moneyLocations:
        dist = math.hypot(location[16] - posX, location[17] - posY)
        if dist < 50:
            print(dist)
            amountMoney += 1

    print("iron")
    #check for ironLocations
    for location in ironLocations:
        dist = math.hypot(location[16] - posX, location[17] - posY)
        if dist < 50:
            print(dist)
            amountIron += 1
    
    print("coal")
    #check for coalLocations
    for location in coalLocations:
        dist = math.hypot(location[16] - posX, location[17] - posY)
        if dist < 50:
            print(dist)
            amountCoal += 1
    amountCoal = int(amountCoal / 2)

    print(amountMoney)
    print(amountCoal)
    print(amountIron)

    station["ironEfficiency"] = amountIron
    station["coalEfficiency"] = amountCoal
    station["moneyEfficiency"] = amountMoney
    
    
mainFile = open("maintest.json", "w", encoding="UTF-8")
json.dump(json_object, mainFile)
mainFile.close()
