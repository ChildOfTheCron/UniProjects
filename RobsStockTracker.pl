#!/usr/bin/perl
#Modules declarations
use strict;
use warnings;
use LWP::Simple;
use Term::ANSIColor qw(:constants);

#Variable declarations
#Get user settings
our @storeUserSettings;
our $URLStock;
our $holdings;
our $stampTax;
our $commish;
our $paidPerShare;
our $stopLoss;

#Variables based on returned data
our $bidPrice;
our $companyName;
our $profitTotal;

our $formatLocalTimeStr;
our $runUntil = "2330";

#Stock maths and working out profits
#Make sure you only use this after you have user settings
#And after you have the parsed data from whatever URL
sub getMarketValue(){
	my $marketVal = $bidPrice*$holdings;
	#print $marketVal."\n";
	return $marketVal;
}
sub getUserShareValue(){
	my $userHoldings = ($paidPerShare*$holdings)+$commish;
	my $stampPercent = ($stampTax/100)*$userHoldings;

	my $userShare = $userHoldings+$stampPercent;
	#print $userShare."\n";
	return $userShare;

}

#Returns the users settings (defined in a text file for now. Pretty hacky but it works for now)
sub getUserInput(){
	open (USERDATA, "<USERDATA.txt") or die 'Unable to get USERDATA';

	foreach( <USERDATA> ){
		push @storeUserSettings, $_; #Save each line from std input to the array
	}

	#print @storeUserSettings;

	#Assign the proper array vals to the scalar variables. This is because it's easier to keep track
	# of where holdings, stampTax etc is rather than keeping track of array indexes
	$holdings = $storeUserSettings[1];
	$stampTax = $storeUserSettings[2];
	$commish = $storeUserSettings[3];
	$paidPerShare = $storeUserSettings[4];
	$stopLoss = $storeUserSettings[5];

	return;
}

#Grab the data from the stock exchange using the user specified URL in USERDATA.txt
#TODO - Remove the HTMLPrint.html, do I really need this even? Try and keep to datastructures?
sub getPage(){
	open (HTML, ">HTMLPrint.html");
	print HTML get($storeUserSettings[0]) or die 'Unable to get page';
	close HTML;

	return;
}

#Start sifting through all the data and get all the juicy bits! Get rid of all the HTML junk
sub formatData(){
	my @storeFormatData; #Used to store all the formatted data

	#Used to do quick matches for specific pieces of HTML
	my $searchFor = '<title>';
	my $searchForTD = '<tbody>';
	my $searchForColour_Red = '<td class="red">';
	my $searchForColour_Green = '<td class="green">';

	#Open the file and read it into a variable
	open INPUT,"<HTMLPrint.html";
	#TODO - Remove this STORE, but keep for now until you finish the getParams() subroutine
	open STORE,">TempStore.txt"; #Used as a temp store, mostly for debugging

	foreach ( <INPUT> ) {
		chomp $_; #Get rid of that annoying newline stuff
		
		#Search for the title of the web page
		if(/$searchFor/){
			s/((\s|<.*?>))//g; #Remove the nasty nasty HTML white space and tags
			#Remove all the newlines because they'll get stored in @storeFormatData and that's no use
			if($_ ne ""){
				#print $_."\n";
				#print STORE $_."\n";
				push @storeFormatData, $_;
			}
		}else{

			#print "Can't find title you're looking for.\n";

		}

		#Search the numbers and number column titles
		if(m/<td>.+<\/td>|<th scope="col" abbr="Details">.+/){
			s/((\s|<.*?>))//g; #Remove the nasty nasty HTML white space and tags
			if($_ ne ""){
				#print $_."\n";
				#print STORE $_."\n";
				push @storeFormatData, $_;
			}

		}else{

			#print "Can't find title you're looking for.\n";

		}

		#Search the green/red tags for those two anoying green vals (+/- and Var%)
		if(m/$searchForColour_Red/ | m/$searchForColour_Green/){
			#s/((\s|<.*?>))//g; #Remove the nasty nasty HTML white space and tags
			if($_ ne ""){
				#print $_."\n";
				#print STORE $_."\n";
				push @storeFormatData, $_;
			}

		}else{

			#DEBUG - Caution this could be printed LOADS of times
			#print "Can't find title you're looking for.\n";

		}
		#Part 2 - search for strings that contain numbers
		if(m/.*(-\d+.\d+%?)/){
			s/((\s|<.*?>))//g; #Remove the nasty nasty HTML white space and tags
			if($_ ne ""){
				#print $_."\n";
				#print STORE $_."\n";
				push @storeFormatData, $_;
			}

		}else{

			#print "Can't find title you're looking for.\n";

		}
	}
	close STORE;
	close INPUT;

	#DEBUG: Use below to see the current formatted share price/bidPrice
	#print "\n";
	#print "***************************\n";
	#print "** stored data at pos 11 **\n";
	#print "***************************\n";
	#print $storeFormatData[11]."\n";
	$bidPrice = $storeFormatData[11];
	$companyName = $storeFormatData[0];
	undef @storeFormatData; #Reset here after storing the values so the next values can be saved (next Company)
	#print "***************************\n";
	#print "\n";

	return;
}

#Will be used to build nice little message boxes for the terminal output
sub buildMsgBoxes(){
	# A quick hacky print jobby just to see profits or losses in pounds sterling
	$profitTotal = (getMarketValue - getUserShareValue)/100;
	
	print WHITE, ON_BLACK, "Current Profits from stocks and shares from ".$companyName." (in pounds) is at:\n";
	print RESET;

	my $rounded = sprintf("%.2f", $profitTotal);
	print $rounded."\n";
	
	#DEBUG: Use Below to print parameters (user specified)
	#print $ARGV[0];
}

sub printTime(){
	#Print the current system time
	my ($sec,$min,$hour,$mday,$mon,$year,$wday,$yday,$isdst)=localtime(time);
	my $now_stringInt = $hour.$min;
	$formatLocalTimeStr = $hour.":".$min;
	
	return $now_stringInt; #Returns numeric val
}

sub calcStopLoss(){
	if($profitTotal <= $stopLoss)
	{
		my $rounded = sprintf("%.2f", $profitTotal);
		print BRIGHT_RED, ON_BLACK, "WARNING: Your profit of $rounded is less than or at your current stop loss set at $stopLoss!\n";
	}
	else{
		print WHITE, ON_BLACK, "Profit is currently higher than set stop loss."
	}
}

#Will be used with $ARGV[x] to get system input params. To see if the user wants compact data or full data output
#BUT FOR NOW I AM USING IT TO TEST STUFFS!
sub getInputAll(){
	#print WHITE, ON_GREEN, "ENTERED getInputAll!";
	my @files = <USERDATA*.txt> or die 'Cant find USERDATA';
	#print @files;
	foreach my $file (@files) {
		#DEBUG: Use below to print each file read in for UserData files
		#print $file . "\n";

		open (NEWUSERDATA, $file) or die "Can't open '$file': $!";
		
		foreach( <NEWUSERDATA> )
		{
			push @storeUserSettings, $_; #Save each line from std input to the array
		}
		close NEWUSERDATA;
		#DEBUG: Use below to see debug output for each time the array is propogated
		#print "DEBUG: ".$storeUserSettings[0];
		#print "DEBUG: ".$storeUserSettings[1];
		#print "DEBUG: ".$storeUserSettings[2];
		#print "DEBUG: ".$storeUserSettings[3];
		#print "DEBUG: ".$storeUserSettings[4];

		$holdings = $storeUserSettings[1];
		$stampTax = $storeUserSettings[2];
		$commish = $storeUserSettings[3];
		$paidPerShare = $storeUserSettings[4];
		$stopLoss = $storeUserSettings[5];

		getPage;
		formatData;

		getMarketValue;
		getUserShareValue;

		buildMsgBoxes;
		calcStopLoss;

		print "\n";
		undef @storeUserSettings; #Reset here
	}
}

sub startHerUp(){
	print BRIGHT_RED, ON_BLACK, "* WELCOME TO ROBDJs Stock Tracker\n";
	print BRIGHT_RED, ON_BLACK, "* Help Option coming soon(tm)!\n";
	print BRIGHT_RED, ON_BLACK, "* Quit out using ctrl+c!\n";
	print RESET;
	print "***************************\n";
	while ($runUntil >= printTime) {

		getInputAll;
		#getUserInput;
		printTime;
		my $printTime = printTime;
		print "***************************\n";
		print BRIGHT_RED, ON_BLACK, "Updated at time: ".$formatLocalTimeStr."\n\n";
		print RESET;
		
		sleep(60);
	}
	print "The time is now: ".$runUntil." I will now stop running.\n Have a great day!";

	return;

}

$SIG{INT} = \&cleanUP;

sub cleanUP
{
	print "***************************\n";
	print "You opted to quit out. I will now stop running.\n Have a great day! \n"; 
	print "***************************\n";
	exit 0;
}

startHerUp;
