(function ( $ ) {
    const fretboardConfigurationsPath = "fretboard-configurations";
    const fretboardPath = "fretboard";
    const chordsPath = "chords";
    const scalesPath = "scales";
    var settings = {};
    var fretboardCalculatorContainerElement;
    var topMenuContainer;
    var twoColumnContainer;
    var fretboardConfigurationsSelect;
    var patternTypesSelect;
    var patternsSelect;
    var keyNoteSelect;
    var noteIndicatorType;
    var fretboardContainer;
    var progressVisible = false;
    var positionHighlight;
    var positionHighlightContainer;
    var optionsContainer;
    var relatedChordsContainer;
    var relatedScalesContainer;
    var viewOptionsContainer;


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

        fretboardCalculatorContainerElement.append('<div id="fretboardContainerProgress" class="row"><img id="fretboardContainerProgressImage" src="/img/progress-image.gif" /></div>');

        fretboardCalculatorContainerElement.append('<div id="topMenuContainer" class="row"></div>');
        topMenuContainer = $('#topMenuContainer');
        topMenuContainer.append('<div class="col-md-6"><p><select id="fretboardConfigurationsSelect" class="form-control form-control-lg"></select></p></div>');
        topMenuContainer.append('<div class="col-md-6"><p><select id="patternTypesSelect" class="form-control form-control-lg"></select></p></div>');
        topMenuContainer.append('<div class="col-md-3"><p><select id="patternsSelect" class="form-control form-control-lg"><option value="">--First Select a Type--</option></select></p></div>');
        topMenuContainer.append('<div class="col-md-3"><p><select id="keyNoteSelect" class="form-control form-control-lg"></select></p></div>');

        fretboardCalculatorContainerElement.append('<div id="twoColumnLayoutContainer"></div>');
        twoColumnContainer = $('#twoColumnLayoutContainer');

        twoColumnContainer.append('<div id="optionsContainer" class="col-md-6"><h3>View Options</h3></div>');
        twoColumnContainer.append('<div id="fretboardContainer" class="col-md-6"></div>');
        optionsContainer = $('#optionsContainer');
        optionsContainer.append('<div id="viewOptionsContainer"><p><select id="noteIndicatorType" class="form-control form-control-lg"></select></p></div>');
        viewOptionsContainer = $('#viewOptionsContainer');
        viewOptionsContainer.append('<div class="col-md-12" id="positionHighlightContainer"><p><select id="positionHighlight" class="form-control form-control-lg"><option value="">--Select a Position--</option></select></p></div>');
        viewOptionsContainer.append('<div class="col-md-12"><p><input type="checkbox" name="fingerings" id="fingeringsCheckbox" /> Show Finger Suggestions</p></div>');

        optionsContainer.append('<h3>Related Chords</h3>');
        optionsContainer.append('<div><p id="relatedChordsContainer"></p></div>');

        optionsContainer.append('<h3>Related Scales</h3>');
        optionsContainer.append('<div><p id="relatedScalesContainer"></p></div>');

        relatedChordsContainer = $('#relatedChordsContainer');
        relatedScalesContainer = $('#relatedScalesContainer');

        optionsContainer.accordion({ collapsible: true, active: false });

        relatedChordsContainer.append('<button type="button" class="btn btn-default">Feature</button>');
        relatedChordsContainer.append('<button type="button" class="btn btn-default">Coming</button>');
        relatedChordsContainer.append('<button type="button" class="btn btn-default">Soon</button>');
        relatedScalesContainer.append('<button type="button" class="btn btn-default">Feature</button>');
        relatedScalesContainer.append('<button type="button" class="btn btn-default">Coming</button>');
        relatedScalesContainer.append('<button type="button" class="btn btn-default">Soon</button>');

        fretboardContainer = $('#fretboardContainer');
        fretboardConfigurationsSelect = $('#fretboardConfigurationsSelect');
        patternTypesSelect = $('#patternTypesSelect');
        patternsSelect = $('#patternsSelect');
        keyNoteSelect = $('#keyNoteSelect');
        noteIndicatorType = $('#noteIndicatorType');
        positionHighlight = $('#positionHighlight');
        positionHighlightContainer = $('#positionHighlightContainer');

        positionHighlightContainer.hide();
        patternTypesSelect.hide();
        patternsSelect.hide();
        keyNoteSelect.hide();
        noteIndicatorType.hide();
        optionsContainer.hide();

        fretboardConfigurationsSelect.change(handleConfigurationsListChanged);
        patternsSelect.change(handlePatternsListChanged);
        patternTypesSelect.change(handlePatternTypesListChanged);
        keyNoteSelect.change(handleKeyNoteListChanged);
        noteIndicatorType.change(handleNoteIndicatorTypeChanged);
        positionHighlight.change(handlePositionHighlightChanged);

        getConfigurationsList();
        getPatternTypesList();
        fillKeyNoteSelect();
        fillNoteIndicatorTypes();
    };

    function handleProgressView() {
        if (!progressVisible) {
            $('#fretboardContainerProgress').height($('#fretboard-calculator').height()-10);
            $('#fretboardContainerProgress').width($('#fretboard-calculator').width());
            $('#fretboardContainerProgress').show();
            progressVisible = true;
        }
    }

    function hideProgressView() {
        if (progressVisible) {
            $('#fretboardContainerProgress').hide();
            progressVisible = false;
        }
    }

    function buildFretboard() {
        var fretboardConfiguration = fretboardConfigurationsSelect.find(":selected").val();
        var patternType = patternTypesSelect.find(":selected").val();
        var pattern = patternsSelect.find(":selected").val();
        var keyNote = keyNoteSelect.find(":selected").val();
        var positionSelected = positionHighlight.find(":selected").val();

        optionsContainer.hide();

        if (fretboardConfiguration != '')
            patternTypesSelect.show();
        else {
            patternTypesSelect.hide();
            patternsSelect.hide();
            keyNoteSelect.hide();
            noteIndicatorType.hide();
            return;
        }

        if (patternType != '')
            patternsSelect.show();
        else {
            patternsSelect.hide();
            keyNoteSelect.hide();
            noteIndicatorType.hide();
            return;
        }

        if (pattern != '') {
            keyNoteSelect.show();
            noteIndicatorType.show();
        }
        else {
            keyNoteSelect.hide();
            noteIndicatorType.hide();
            return;
        }

        if (patternType == "Scales")
            patternType = "scale";

        if (patternType == "Chords")
            patternType = "chord";

        fretboardContainer.html('');

        if (fretboardConfiguration != '' && pattern != '' && keyNote != '') {
            var newPath = fretboardPath + "/" + fretboardConfiguration + "/" +
            patternType + "/" + pattern + "/" + keyNote;

        if (positionSelected != "")
            newPath = newPath + "/" + positionSelected;

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
        handleProgressView();
        $.getJSON(endpoint, null, func);
    }

    function handleFretboardRender(json) {
        var indicatorType = noteIndicatorType.find(":selected").val();
        var usedNotes = [];
        positionHighlight.html('');
        positionHighlight.append('<option value="">--Select Position--</option>');
        for (var x = 0; x < json.usedNotes.length; x++) {
            var selectedText = "";
            usedNotes.push(json.usedNotes[x].noteValue);

            if (json.positionHighlight == json.usedNotes[x].positionValue)
                selectedText = " selected";

            positionHighlight.append('<option value="' + json.usedNotes[x].positionValue + '"' + selectedText + '>' + json.usedNotes[x].positionName + '</option>');
        }

        fretboardContainer.append('<div id="fretboard"></div>');
        var fretboardElement = $('#fretboard');

        for (var x = 0; x < json.strings.length; x++) {
            fretboardElement.append('<div id="string' + x + '" class="string"></div>');

            var fretboardString = $('#string' + x.toString())
            for (var y = 0; y < json.strings[x].frets.length; y++) {
                var fretValue = "&nbsp;";
                var selectedClass = "";

                if (y == 0)
                    fretValue = json.strings[x].frets[y].noteName;

                if (y == 0 && usedNotes.includes(json.strings[x].frets[y].note))
                    selectedClass = " usedNote";

                if (y != 0 && usedNotes.includes(json.strings[x].frets[y].note)) {
                    switch (indicatorType) {
                        case "Dots":
                            fretValue = '&#9679;';
                            break;
                        default:
                            fretValue = json.strings[x].frets[y].noteName;
                    }    
                }

                fretboardString.append('<div id="string' + x + '-fret' + y + '" class="fret' + selectedClass + '">' + fretValue + '</div>');
            }
        }
        optionsContainer.show();
        positionHighlightContainer.show();

        hideProgressView();
    }

    function handleConfigurationsList(json) {
        fretboardConfigurationsSelect.append('<option value="">--Select Fretboard Configuration--</option>');
        for (var x = 0; x < json.length; x++) {
            fretboardConfigurationsSelect.append('<option value="' + json[x] + '">' + json[x] + '</option>');
        }
        hideProgressView();
    }

    function handleConfigurationsListChanged() {
        fretboardConfigurationsSelect.children().first().attr('disabled','true');
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
        fretboardContainer.html('');
        keyNoteSelect.hide();
        optionsContainer.hide();
        var patternType = patternTypesSelect.find(":selected").val();
        patternTypesSelect.children().first().attr('disabled','true');
        patternsSelect.html('');
        patternsSelect.append('<option value="">--Select Pattern--</option>');
        getPatternsList(patternType);
        patternsSelect.show();
    }

    function handlePatternsList(json) {
         patternsSelect.html('');
         patternsSelect.append('<option value="">--Select Pattern--</option>');
         for (var x = 0; x < json.length; x++) {
             patternsSelect.append('<option value="' + json[x] + '">' + json[x] + '</option>');
         }
         hideProgressView();
    }

    function handlePatternsListChanged() {
        patternsSelect.children().first().attr('disabled','true');
        buildFretboard();
    }

    function handleKeyNoteListChanged() {
        buildFretboard();
    }

    function handleNoteIndicatorTypeChanged() {
        buildFretboard();
    }

    function handlePositionHighlightChanged() {
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

    function fillNoteIndicatorTypes() {
        noteIndicatorType.append('<option value="Dots">Show As Dot Indicators</option>');
        noteIndicatorType.append('<option value="Notes">Show As Note Names</option>');
    }
}( jQuery ));

