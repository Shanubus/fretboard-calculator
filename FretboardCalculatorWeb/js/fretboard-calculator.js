(function ( $ ) {
    const fretboardConfigurationsPath = "fretboard-configurations";
    const fretboardPath = "fretboard";
    const chordsPath = "chords";
    const scalesPath = "scales";
    var settings = {};
    var fretboardCalculatorContainerElement;
    var fretboardConfigurationsSelect;
    var patternTypesSelect;
    var patternsSelect;
    var keyNoteSelect;
    var fretboardContainer;

    $.fn.fretboardCalculator = function( options ) {
        fretboardCalculatorContainerElement = this;

        settings = $.extend({
            fretboardConfigurationName: null,
            patternName: null,
            patternKeyNote: null,
            endpoint: null
        }, options );

        if (settings.endpoint == null) {
            noEnpoint(this);
            return;
        }

        fretboardCalculatorContainerElement.append('<p class="select-list-container"><select id="fretboardConfigurationsSelect" class="form-control form-control-lg"></select></p>');
        fretboardCalculatorContainerElement.append('<p class="select-list-container"><select id="patternTypesSelect" class="form-control form-control-lg"></select></p>');
        fretboardCalculatorContainerElement.append('<p class="select-list-container"><select id="patternsSelect" class="form-control form-control-lg"><option value="">--First Select a Type--</option></select></p>');
        fretboardCalculatorContainerElement.append('<p class="select-list-container"><select id="keyNoteSelect" class="form-control form-control-lg"></select></p>');
        fretboardCalculatorContainerElement.append('<div id="fretboardContainer"></div>');

        fretboardContainer = $('#fretboardContainer');
        fretboardConfigurationsSelect = $('#fretboardConfigurationsSelect');
        patternTypesSelect = $('#patternTypesSelect');
        patternsSelect = $('#patternsSelect');
        keyNoteSelect = $('#keyNoteSelect');

        fretboardConfigurationsSelect.change(handleConfigurationsListChanged);
        patternsSelect.change(handlePatternsListChanged);
        patternTypesSelect.change(handlePatternTypesListChanged);
        keyNoteSelect.change(handleKeyNoteListChanged);

        getConfigurationsList();
        getPatternTypesList();
        fillKeyNoteSelect();
    };

    function buildFretboard() {
        var fretboardConfiguration = fretboardConfigurationsSelect.find(":selected").val();
        var patternType = patternTypesSelect.find(":selected").val();
        var pattern = patternsSelect.find(":selected").val();
        var keyNote = keyNoteSelect.find(":selected").val();

        if (patternType == "Scales")
            patternType = "scale";

        if (patternType == "Chords")
            patternType = "chord";

        fretboardContainer.html('');

        if (fretboardConfiguration != '' && pattern != '' && keyNote != '') {
            fretboardContainer.append('<h3>' + fretboardConfiguration + 
            ' Fretboard with ' + pattern + ' pattern in the key of ' + keyNote + '</h3>');
            
            var newPath = fretboardPath + "/" + fretboardConfiguration + "/" +
            patternType + "/" + pattern + "/" + keyNote;

            getJsonFromApi(settings.endpoint + newPath, handleFretboardRender);
        }
    }

    function getConfigurationsList() {
        getJsonFromApi(settings.endpoint + fretboardConfigurationsPath, handleConfigurationsList);
    }

    function getPatternTypesList() {
        var patternTypes = ["Chords","Scales"];
        handlePatternTypesList(patternTypes);
    }

    function getPatternsList(patternType) {
        var patternPath = "";
        if (patternType == "Chords")
            patternPath = chordsPath;

        if (patternType == "Scales")
            patternPath = scalesPath;

        getJsonFromApi(settings.endpoint + patternPath, handlePatternsList);
    }

    function noEnpoint() {
        fretboardCalculatorContainerElement.html('Fretboard Calculator: Endpoint Must Be Provided');
    }

    function getJsonFromApi(endpoint, func) {
        $.getJSON(endpoint, null, func);
    }

    function handleFretboardRender(json) {
        var usedNotes = [];
        for (var x = 0; x < json.usedNotes.length; x++) {
            usedNotes.push(json.usedNotes[x].noteValue);
        }

        fretboardContainer.append('<div id="fretboard"></div>');
        var fretboardElement = $('#fretboard');

        for (var x = 0; x < json.strings.length; x++) {
            fretboardElement.append('<div id="string' + x + '" class="string"></div>');
            var fretboardString = $('#string' + x.toString())
            for (var y = 0; y < json.strings[x].frets.length; y++) {
                var fretValue = "&nbsp;";
                var selectedClass = "";
                if (y == 0) {
                    fretValue = json.strings[x].frets[y].noteName;
                }

                if (y == 0 && usedNotes.includes(json.strings[x].frets[y].note))
                    selectedClass = " usedNote";

                if (y != 0 && usedNotes.includes(json.strings[x].frets[y].note))
                    fretValue =json.strings[x].frets[y].noteName;

                fretboardString.append('<div id="string' + x + '-fret' + y + '" class="fret' + selectedClass + '">' + fretValue + '</div>');
            }
        }
    }

    function handleConfigurationsList(json) {
        fretboardConfigurationsSelect.append('<option value="">--Select Fretboard Configuration--</option>');
        for (var x = 0; x < json.length; x++) {
            fretboardConfigurationsSelect.append('<option value="' + json[x] + '">' + json[x] + '</option>');
        }
    }

    function handleConfigurationsListChanged() {
        buildFretboard();
    }

    function handlePatternTypesList(json) {
        patternTypesSelect.html('');
        patternTypesSelect.append('<option value="">--Select Chords or Scales--</option>');
        for (var x = 0; x < json.length; x++) {
            patternTypesSelect.append('<option value="' + json[x] + '">' + json[x] + '</option>');
        }
    }

    function handlePatternTypesListChanged() {
        var patternType = patternTypesSelect.find(":selected").val();
        patternsSelect.html('');
        patternsSelect.append('<option value="">--Select Pattern--</option>');
        getPatternsList(patternType);
    }

    function handlePatternsList(json) {
         patternsSelect.html('');
         patternsSelect.append('<option value="">--Select Pattern--</option>');
         for (var x = 0; x < json.length; x++) {
             patternsSelect.append('<option value="' + json[x] + '">' + json[x] + '</option>');
         }
    }

    function handlePatternsListChanged() {
        buildFretboard();
    }

    function handleKeyNoteListChanged() {
        buildFretboard();
    }

    function fillKeyNoteSelect() {
        keyNoteSelect.append('<option value="C">C</option>');
        keyNoteSelect.append('<option value="Csharp">C#</option>');
        keyNoteSelect.append('<option value="Dflat">Db</option>');
        keyNoteSelect.append('<option value="D">D</option>');
        keyNoteSelect.append('<option value="Dsharp">D#</option>');
        keyNoteSelect.append('<option value="Eflat">Eb</option>');
        keyNoteSelect.append('<option value="E">E</option>');
        keyNoteSelect.append('<option value="F">F</option>');
        keyNoteSelect.append('<option value="Fsharp">F#</option>');
        keyNoteSelect.append('<option value="Gflat">Gb</option>');
        keyNoteSelect.append('<option value="G">G</option>');
        keyNoteSelect.append('<option value="Gsharp">G#</option>');
        keyNoteSelect.append('<option value="Aflat">Ab</option>');
        keyNoteSelect.append('<option value="A">A</option>');
        keyNoteSelect.append('<option value="Asharp">A#</option>');
        keyNoteSelect.append('<option value="Bflat>Bb</option>');
        keyNoteSelect.append('<option value="B">B</option>');
    }

    function getNoteName(noteValue) {
        var flatted = false;
        
        switch (noteValue) {
            case 0:
                return "C";
                break;
            case 0.5:
                if (flatted)
                    return "Db"
                return "C#";
                break;
            case 1:
                return "D";
                break;
            case 1.5:
                if (flatted)
                    return "Eb"
                return "D#";
                break;
            case 2:
                return "E";
                break;
            case 2.5:
                return "F";
                break;
            case 3.0:
                if (flatted)
                    return "Gb"
                return "F#";
                break;
            case 3.5:
                return "G";
                break;
            case 4:
                if (flatted)
                    return "Ab"
                return "G#";
                break;
            case 4.5:
                return "A";
                break;
            case 5:
                if (flatted)
                    return "Bb"
                return "A#";
                break;
            case 5.5:
                return "B";
                break;
            default:
                return "";
        }
    }
}( jQuery ));

