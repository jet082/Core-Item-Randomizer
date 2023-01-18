import json, random

itemList = set()
itemListFull = [set()]

def ParseRandom(itemLogic, isOr=True):
		if isinstance(itemLogic, str):
			if itemLogic in gameLogic['logic']:
				ParseRandom(gameLogic['logic'][itemLogic])
			else:
				itemList.add(itemLogic)
		else:
			if isOr:
				toParseRandom = random.choice(itemLogic)
				ParseRandom(toParseRandom, not isOr)
			else:
				for someCondition in itemLogic:
					ParseRandom(someCondition)
def ParseFull(itemLogic, setIndex=0, isOr=True):
		if isinstance(itemLogic, str):
			if itemLogic in gameLogic['logic']:
				ParseFull(gameLogic['logic'][itemLogic], setIndex)
			else:
				itemListFull[setIndex].add(itemLogic)
		else:
			if isOr:
				for someOrCondition in itemLogic:
					itemListFull.append(itemListFull[setIndex])
					ParseFull(someOrCondition, setIndex + 1, not isOr)
			else:
				for someCondition in itemLogic:
					ParseFull(someCondition, setIndex)

with open("DefaultLogic.json") as f:
	gameLogic = json.loads(f.read())
	keyToParseRandom = random.choice(list(gameLogic['supplyCrateCoordinates'].keys()))
	ParseRandom(gameLogic['supplyCrateCoordinates'][keyToParseRandom]['logic'])
	ParseFull(gameLogic['supplyCrateCoordinates'][keyToParseRandom]['logic'])
print(keyToParseRandom)
print(itemList)
print(itemListFull)