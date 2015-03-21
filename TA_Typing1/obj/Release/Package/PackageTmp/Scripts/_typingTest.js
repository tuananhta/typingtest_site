//the file for typing test application

var start = false;          // to check if the program started
var stop = false;           // to check if the program stopped
var randNum = 0;
var numChar = 0;            // total of characters are typed
var correctWords = 0;       // total of correct words
var totalWords = 0;
var sizeBox = 120;          // total characters of the box
var crtIndex = 0;           // current index
var fixedSeconds = 20;
var seconds = 0;

inpMsg = [];                // the input message

// start program
$(document).ready(function () {
    refresh();
});

function reload() {
    location.reload();
}

function refresh() {
    seconds = fixedSeconds;
    totalWords = 0;
    correctWords = 0;
    numChar = 0;
    start = false;
    stop = false;

    genMsg();
    $('#seconds').empty();
    $('#seconds').append(seconds);
    $('#typed').keydown(function () {
        if (start === false) {
            startPrg();
            timer();
            start = true;
        }
    });
}

// count time
function countTime() {
    seconds--;

    $('#seconds').empty();
    if (seconds < 10) {
        $('#seconds').append(0);
    }
    $('#seconds').append(seconds);

    timer();
}

function timer() {
    if (seconds > 0) {
        t = setTimeout(countTime, 1000);
    }
    else {
        // do when time is over
        stopPrg();
    }
}
// end of count time

// Function to generate words
function genMsg() {
    delete inpMsg;
    inpMsg = [];
    crtIndex = 0;

    totalChars = 0;
    mssgg = "";

    while (totalChars <= sizeBox) {
        do {
            newRand = Math.floor((Math.random() * msg.length)) % msg.length;
        }
        while (randNum === newRand)
        randNum = newRand;

        if (totalChars + msg[randNum].length > sizeBox)
            break;

        inpMsg.push(msg[randNum]);
        totalChars += msg[randNum].length + 1; // plus a space
    }

    for (i = 0; i < inpMsg.length; ++i) {
        mssgg += "<span id='word" + i + "' class='btn btn-default text-center word-input disabled'>" + inpMsg[i] + "</span> ";
    }
    // clear the div tag
    $('#given').empty();
    // append into the container
    $('#given').append(mssgg);
    // paint color for the 1st element
    //$(word).removeClass("btn-default");
    $('#word0').addClass("btn-primary disabled");
}

function startPrg() {
    $('#typed').keydown(function (evt) {
        // total of input characters
        numChar++;
        $('#result3').val(numChar);

        if (evt.which === 32 && stop == false) {
            // total of words user has typed
            totalWords++;
            // update info
            $('#result2').val(totalWords);
            $('#result1').val(correctWords);

            // assign the class for the current index
            word = "#word" + crtIndex;
            //$(word).removeClass("btn-primary");
            $(word).addClass("btn-default disabled");

            // check the condition if the user type right
            if ($('#typed').val() === inpMsg[crtIndex]) {
                correctWords++;
                $('#result1').val(correctWords);

                // paint the color for the correct word
                //$(word).removeClass("current");
                $(word).addClass("btn-success disabled");
            }
            else
                // paint the color for the wrong word
                $(word).addClass("btn-danger disabled");

        }
    })

    $('#typed').keyup(function (evt) {
        if (evt.which === 32 && stop == false) {
            $('#typed').val("");
            if (crtIndex < inpMsg.length - 1) {
                crtIndex = crtIndex + 1;
                word = "#word" + crtIndex;
                // highlight the current word
                $(word).addClass("btn-primary disabled");
            }
            else
                // if the text sample is exceeded, renew
                genMsg();
        }
    });
}