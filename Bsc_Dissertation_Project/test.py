import sys
import nltk
import pyodbc
from easygui import *
from nltk import load_parser
from decimal import *

def getNextBest(recordVal):
	print 'getNextBest reached!'
	compareList = []
	compareList = recordVal
	#compareList[0] = row.DisplayScore
	#compareList[1] = row.DisplayName
	#compareList[2] = row.ID
	#compareList[3] = row.PID
	conn = pyodbc.connect('DRIVER={SQL Server}; SERVER=HOBO-PC; DATABASE=DisplayDB') #Connection to the database
	cursor = conn.cursor() #defines a new cursor
	cursor.execute("SELECT PID, DisplayName, ID, DisplayScore FROM KnowledgeBase WHERE PID = ?", compareList[3])
	rows = cursor.fetchall()
	newRecord = [0, "None", 0, 0]
	for row in rows:
		if compareList[0] >= row.DisplayScore and row.ID <> compareList[2] and row.DisplayScore > newRecord[0]:
			newRecord[0] = row.DisplayScore
			newRecord[1] = row.DisplayName
			newRecord[2] = row.ID
			newRecord[3] = row.PID
	askUser(newRecord)

def updateScoreDec(scoreVal, idVal):
	print 'updateScoreDEC reached'
	try:
		if scoreVal > 0:
			#newScore = val + Decimal((0, (0, 1), -2))
			newScore = scoreVal - Decimal((0, (0, 1), -1)) #Increment the Score by 0.1
			idUpdate = idVal
		else:
			newScore = 0
			idUpdate = idVal
		
		conn = pyodbc.connect('DRIVER={SQL Server}; SERVER=HOBO-PC; DATABASE=DisplayDB') #Connection to the database
		cursor = conn.cursor() #defines a new cursor
		cursor.execute("UPDATE KnowledgeBase SET DisplayScore = ? WHERE ID = ?", newScore, idUpdate ).rowcount #Get ID and get newScore
		conn.commit()
	except:
		print 'except statement in updateScoreDec executed!'

def updateScoreInc(scoreVal, idVal):
	try:
		#newScore = val + Decimal((0, (0, 1), -2))
		newScore = scoreVal + Decimal((0, (0, 1), -1)) #Increment the Score by 0.1
		idUpdate = idVal
	
		conn = pyodbc.connect('DRIVER={SQL Server}; SERVER=HOBO-PC; DATABASE=DisplayDB') #Connection to the database
		cursor = conn.cursor() #defines a new cursor
		cursor.execute("UPDATE KnowledgeBase SET DisplayScore = ? WHERE ID = ?", newScore, idUpdate ).rowcount #Get ID and get newScore
		conn.commit()
	except:
		print 'except statement in updateScore executed!'
	
def parseAgain(text):
	inputAgain = enterbox(msg=text, title='Sqelca', default='', strip=True, image="SQLCA.gif", root=None)
	parseInput(inputAgain)

# This function returns the appropriote response to the question/query asked.
# In this build it is not used, yet, except by the try - catch in parseInput
# NOT USED AT THE MOMENT - SUCCEEDED by parseAgain function
# ####################################################################
def resultMsgBox(result):
	result = enterbox(msg=result, title='Sqelka', default='', strip=True, image=None, root=None)

# UNFINISHED
# This will prompt the user asking them if they are intrested in x display.
# x display is derived from the Scores in the getData function
# The user will be able to select Yes or No depending if the result is the one
# they were intrested in.
# Scores will be updated accordingly.
# ####################################################################
def askUser(param):
	asklist = []
	asklist = param #bad idea, what happens if the user asks another Q? the list grows, not gonna work, overwrite
	#asklist[0] = row.DisplayScore
	#asklist[1] = row.DisplayName
	#asklist[2] = row.ID
	#asklist[3] = row.PID
	response = enterbox(image="SQLCA.gif", msg='I think you are trying to get to the ' + asklist[1] + ' display.\nAm I right in thinking so?', title='Sqelca', default='', strip=True, root=None)
	
	response = filter(lambda c: c not in "?", response)
	#print 'askUser() >> user said: ' + response
	if response == 'Goodbye':
		exit() #Exits the system
	else:
		#print 'askUser() >> Parsing in AskUser at the moment...'
		grammar1 = load_parser('file:C:\Users\Hobo\Desktop\FinalYearProj\projGrammar.fcfg')
		try:
			query = response
			trees = grammar1.nbest_parse(query.split())
			answer = trees[0].node['SEM']
			q = ' '.join(answer)
			
			if q == 'Rejection':
				print 'rejection area reached'
				#idInt = asklist[2]
				updateScoreDec(asklist[0], asklist[2])
				getNextBest(asklist)

			else:
				#q == 'Affirmation':
				idInt = asklist[2]
				conn = pyodbc.connect('DRIVER={SQL Server}; SERVER=HOBO-PC; DATABASE=DisplayDB') #Connection to the database
				cursor = conn.cursor() #defines a new cursor
				cursor.execute("SELECT Description FROM KnowledgeBase WHERE ID = ?", idInt)
				rows = cursor.fetchone()
				updateScoreInc(asklist[0], asklist[2])
				parseAgain(rows)
		except:
			parseAgain('I do not understand that, sorry!')
			
# After the users question is parsed into SQL it is used to return relevent data from the DB
# Once the data is returned it is compared to a list so that the highest Score is selected
# from all of the relevent data.
# ####################################################################
def getData(sql): #This works! DO NOT try and connect at start of script! Won't work! Keep connect here!
	if sql == 'Rejection':
		parseAgain('I do not understand what you are disagreeing with.\nWe have not yet begun to converse.')
	elif sql == 'Affirmation':
		parseAgain('I do not understand what you are agreeing with.\nI have not said anything yet.')
	else:
		conn = pyodbc.connect('DRIVER={SQL Server}; SERVER=HOBO-PC; DATABASE=DisplayDB') #Connection to the database
		cursor = conn.cursor() #defines a new cursor
		cursor.execute(sql)
		row = cursor.fetchone()
		idInt = row
	
		cursor.execute("SELECT PID, DisplayName, ID, DisplayScore FROM KnowledgeBase WHERE PID = ?", idInt)
		rows = cursor.fetchall()
		index = [0, "None", 0, 0]
		for row in rows:
			if row.DisplayScore >= index[0]:
				index[0] = row.DisplayScore
				index[1] = row.DisplayName
				index[2] = row.ID
				index[3] = row.PID
		askUser(index)

#This is the parser, it parses the users query into an SQL statement
#ie: Converting from Natural Language(kinda) into SQL
#Try - Catch is used to ensure the system doesn't crash if a user enters incorrect questions
#or questions it doesn't understand.
# ####################################################################
def parseInput(text):
	#Line below removes any ? marks since my grammar can't deal with those.
	#Also, it's much easier to remove question marks in code than to
	#create a grammar that understands them!
	text = filter(lambda c: c not in "?", text)
	if text == 'Goodbye':
		#conn.close()  --not used yet, was used in earlier builds but now I need to call this later if at all
		exit() #Exits the system
	else:
		grammar1 = load_parser('file:C:\Users\Hobo\Desktop\FinalYearProj/projGrammar.fcfg')
		try:
			query = text
			trees = grammar1.nbest_parse(query.split())
			answer = trees[0].node['SEM']
			q = ' '.join(answer)
			print q
			getData(q)
		except:
			print 'parseInput exception thrown'
			parseAgain('I do not understand that, sorry!')

#The main function, that fires off the chain of events
#print reply -- prints the users question to console [USED FOR DEBUGGING]
#parseInput(reply) -- calls the parseInput function passing the users query as a paramater
# ####################################################################			
def main():
	reply = enterbox(image="SQLCA.gif", msg='Hello my name is Sqelca. How can I help you, today?', title='Sqelca', default='', strip=True, root=None) 
	parseInput(reply)
	
#This is the start of the script, the isopen being set to one causes an infinite loop that can only be
#exited by using exit().
# ####################################################################
isopen = 1
while isopen == 1: #only stops if the user says "Goodbye"
main() #main function is called here