from flask import Flask, url_for, Response, json, request
from enum import Enum

app = Flask(__name__)

@app.route('/')
def api_root():
    return 'ok'

@app.route('/info', methods = ['GET'])
def api_info():     
    js = infoObject.toJSON()
    resp = Response(js, status=200, mimetype='application/json')
    return resp

@app.route('/PerformNextMove', methods = ['POST'])
def api_performNextMove():
    battlefieldInfoJSON = json.loads(request.data)
    battlefieldInfo = BattlefieldInfoClass()
    battlefieldInfo.loadFromJson(battlefieldInfoJSON)
    
    js = gamer.calculateNextMove(battlefieldInfo)
    resp = Response(js, status=200, mimetype='application/json')
    return resp
    
class InfoObject():
    Name = ""
    AvatarUrl = ""
    Decription = ""
    GameType = ""
    
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
            sort_keys=True, indent=4)

class BombClass():
    RoundsUntilExplodes = 0
    Location = ""
    ExplosionRadius = 0

    def loadFromJson(self,jsonObject):
        self.RoundsUntilExplodes = jsonObject['RoundsUntilExplodes']
        self.Location = jsonObject['Location']
        self.ExplosionRadius = jsonObject['ExplosionRadius']
        

class MissilesClass():
    MoveDirection = 0
    Location = ""
    ExplosionRadius = 0

    def loadFromJson(self,jsonObject):
        self.MoveDirection = jsonObject['MoveDirection']
        self.Location = jsonObject['Location']
        self.ExplosionRadius = jsonObject['ExplosionRadius']

class GameConfigClass():
    def loadFromJson(self, jsonObject):
        self.MapWidth = jsonObject['MapWidth']
        self.MapHeight = jsonObject['MapHeight']
        self.BombBlastRadius = jsonObject['BombBlastRadius']
        self.MissileBlastRadius = jsonObject['MissileBlastRadius']
        self.RoundsBetweenMissiles = jsonObject['RoundsBetweenMissiles']
        self.RoundsBeforeIncreasingBlastRadius = jsonObject['RoundsBeforeIncreasingBlastRadius']
        self.BombRoundsUntilExplodes = jsonObject['BombRoundsUntilExplodes']
    
class BattlefieldInfoClass():
    RoundNumber = 0
    TurnNumber = 0
    Board = []
    BotLocation = ""
    MissileAvailableIn = 0
    OpponentLocations = 0
    GameConfig = GameConfigClass()

    def loadFromJson(self,jsonObject):
        self.RoundNumber = jsonObject['RoundNumber']
        self.TurnNumber = jsonObject['TurnNumber']
        self.Board = jsonObject['Board']
        self.BotLocation = jsonObject['BotLocation']
        self.MissileAvailableIn = jsonObject['MissileAvailableIn']
        self.OpponentLocations = jsonObject['OpponentLocations']

        for bomb in jsonObject["Bombs"]:
            _bomb = BombClass()
            _bomb.loadFromJson(bomb)
            self.Bombs.append(_bomb)

        for missile in jsonObject["Missiles"]:
            _missile = MissilesClass()
            _missile.loadFromJson(missile)
            self.Missiles.append(_missile)
            
        self.GameConfig.loadFromJson(jsonObject["GameConfig"])
        
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
            sort_keys=True, indent=4)
    
    def __init__(self):
        self.Bombs = []
        self.Missiles = []
        self.GameConfig = GameConfigClass()

class BotMoveClass():
    def __init__ (self, dir,act,firdir):
        self.Direction = dir
        self.Action = act
        self.FireDirection = firdir

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
            sort_keys=True, indent=4)

Directions = {'Up':0, 'Down':1, 'Right':2, 'Left':3}
BotAction = {'None':0, 'DropBomb':1, 'FireMissile':2}
BoardTile = {'Empty':0, 'Regular':1, 'Fortified':2, 'Indestructible':3}
    
class Gamer():        
    def calculateNextMove(self, battlefield):
        #Oblicz jako ma być ruch - ale to już trzeba samemu zrobić ;) 
        move = BotMoveClass(Directions['Up'], BotAction['FireMissile'], Directions['Right'])

        return move.toJSON()
    
  
#Uzupełnić info  
infoObject = InfoObject()
infoObject.Name = ''
infoObject.AvatarUrl = ''
infoObject.Decription = ''
infoObject.GameType = ''

if __name__ == '__main__':
    gamer = Gamer()
    app.run(host='127.0.0.1', port=6666) #zmienic na odpwiednie
