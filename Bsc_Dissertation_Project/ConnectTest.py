import pyodbc
import nltk
from nltk import load_parser

grammar1 = load_parser('file:C:\Users\Hobo\Documents\UniWork\FinalYearProj/projGrammarTEST.fcfg')
try:
	query = "Where can I find displays about CrystalBox"
	trees = grammar1.nbest_parse(query.split())
	answer = trees[0].node['SEM']
	q = ' '.join(answer)
	print q
except:
	print "crashed"

conn = pyodbc.connect('DRIVER={SQL Server}; SERVER=HOBO-PC; DATABASE=DisplayDB') #Connection for the database
cursor = conn.cursor() #defines a new cursor
#q = "SELECT    Description FROM KnowledgeBase WHERE  DisplayName='CrystalBox'"
#print q
#p = q
cursor.execute(q)
row = cursor.fetchone()
print row
raw_input('Press ENTER to continue...\n')