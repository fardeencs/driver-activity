
const columnSearchUtil = (function () {
    'use strict';

    const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const actionSearchEvent = (ctrlId, callback, event, params) => {
        let ctlValue = {};
        $('#' + ctrlId + ', .search-control').each(function () {
            const val = $(this).val();
            const dataKey = $(this).attr('data-key');
            const ctrlType = $(this).attr('type') ?? $(this).prop('nodeName');;
            if (ctrlType && ctrlType == 'checkbox') {
                ctlValue[dataKey] = $(this).prop('checked');
            }
            else if (val && ctrlType && ctrlType == 'number') {
                const numberVal = Number(val ?? 0);
                if (numberVal > 0)
                    ctlValue[dataKey] = numberVal;
            }
            else if (ctrlType && ctrlType?.toLocaleLowerCase() == 'select' && $(this).attr('multiple')) {
                if (val && val?.length > 0)
                    ctlValue[dataKey] = val;
            }
            else if(val && val.trim().length > 0 && dataKey) {
                ctlValue[dataKey] = val.trim();
            }
        });
        if (_.isEmpty(ctlValue)) return;

        callback({ data: ctlValue, event, params });
    };

    const clearSearchEvent = (ctrlId, callback, event, params) => {
        $('#' + ctrlId + ', .search-control').each(function () {
            $(this).val('');
        });
        if (callback)
            callback({ event, params });
    };

    const createControl = (col, rowId, placeholderCtrl) => {
        const { type, field, header, collections, value, prefix, suffix, multiple } = col;
        const cell = document.createElement('div');
        const cellId = rowId + '-cell-' + field;
        cell.id = cellId;
        cell.style.padding = '5px';
        const cellClazz1 = 'cell-' + field;
        placeholderCtrl = placeholderCtrl ?? " Seach by {0}";
        const placeholderCtrlValue = commonUtil.stringFormat(placeholderCtrl, header);
        //const classesToAdd = [cellClazz1, 'col-md-3', 'col-ms-6', 'div-search-control'];
        const classesToAdd = [cellClazz1, 'div-search-control'];
        cell.classList.add(...classesToAdd);
        const cellVal = value || null;
        switch (type) {
            case 'TEXT_BOX':
                const textBox = document.createElement('input');
                textBox.id = cellId + "-textbox-" + field;
                textBox.type = "text";
                //textBox.placeholder = header;
                textBox.classList.add("textbox-" + field, 'search-control', 'form-control');
                textBox.value = cellVal;
                //textBox['data-key'] = field;
                textBox.setAttribute("data-key", field);
                textBox.setAttribute("placeholder", placeholderCtrlValue);
                cell.appendChild(textBox);
                break;
            case 'TEXT_AREA':
                const textArea = document.createElement('textarea');
                textArea.id = cellId + "-textarea-" + field;
                textArea.rows = col['rows'] || 2;
                textArea.setAttribute("placeholder", placeholderCtrlValue);
                textArea.classList.add("textarea-" + field, 'search-control', 'form-control');
                textArea.value = cellVal;
                //textArea['data-key'] = field;
                textArea.setAttribute("data-key", field);
                cell.appendChild(textArea);
                break;
            case 'TEXT':
                cell.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                break;
            case 'NUMBER_BOX':
                const numberBox = document.createElement('input');
                numberBox.id = cellId + "-numberbox-" + field;
                numberBox.type = "number";
                numberBox.setAttribute("placeholder", placeholderCtrlValue);
                numberBox.classList.add("numberbox-" + field, 'search-control', 'form-control');
                numberBox.value = cellVal;
                //numberBox['data-key'] = field;
                numberBox.setAttribute("data-key", field);
                cell.appendChild(numberBox);
                break;
            case 'NUMBER':
                cell.innerHTML = `${(prefix || '')} ${formatter.format(cellVal || 0)} ${(suffix || '')}`;
                break;
            case 'DATE':
                break;
            case 'DATE_TIME':
                break;
            case 'DATE_BOX':
                //data-key="${field}" 
                const dateBoxId = cellId + "-datebox-" + field;
                /*
                const dateBox = `<div class="input-group date" data-provide="datepicker">
                                    <input type="text" id="datebox-${dateBoxId}" data-key="${field}" class="search-control form-control" placeholder="${header}">
                                    <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-th"></span>
                                    </div>
                                 </div>`;
                                 */
                const dateBox = document.createElement('input');
                dateBox.id = dateBoxId;
                dateBox.type = "text";
                dateBox.setAttribute("placeholder", placeholderCtrlValue);
                dateBox.setAttribute("data-key", field);
                dateBox.classList.add("datebox-" + field, 'search-control', 'form-control', 'datepicker');
                dateBox.value = cellVal;
                cell.appendChild(dateBox);
                //const dateBox = `<input type="text" id="datebox-${dateBoxId}" data-key="${field}" class="search-control form-control datepicker">`;
                /* const dateBox = `<div class="date input-wrapper" data-provide="datepicker">
                                  <input type="text" id="datebox-${dateBoxId}" class="search-control form-control" placeholder=${header}>
                                 </div>`;*/

                //cell.innerHTML = dateBox;
                break;
            case 'DROPDOWN':
                const select = document.createElement('select');
                select.id = cellId + "-selectbox-" + field;
                select.setAttribute("placeholder", header);
                //select['data-key'] = field;
                select.setAttribute("data-key", field);
                if (multiple) {
                    select.multiple = "multiple";
                }
                select.classList.add("select2", "selectbox-" + field, 'search-control', 'form-control');
                const placeholderOption = document.createElement('option');
                //placeholderOption.disabled = true;
                //placeholderOption.selected = true;
                //placeholderOption.text = header;
                select.appendChild(placeholderOption);
                collections.forEach((item, i) => {
                    const option = document.createElement('option');
                    option.text = item.label;
                    option.value = item.value;
                    select.appendChild(option);
                });
                cell.appendChild(select);
                break;
            default:
                cell.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                break;
        }

        return cell;
    };

    const createColumnSearch = ({ appendId, ctrlId, colDef, type, actions, params }) => {
        const controlsRow = document.createElement('div');
        controlsRow.id = `controls-${ctrlId}`;
        controlsRow.classList.add('app-column-search');

        colDef.forEach((col, index) => {
            controlsRow.appendChild(createControl(col, index));
        });

        //$colSearch.append(controlsRow);
        //var datepicker = $.fn.datepicker.noConflict(); // return $.fn.datepicker to previously assigned value
        // $.fn.bootstrapDP = datepicker;

        const actionsRow = document.createElement('div');
        actionsRow.id = `actions-${ctrlId}`;
        actionsRow.classList.add('actions-search-panel');
        controlsRow.classList.add('actions-column-search', 'row-align-wrap');
        actions.forEach((item, index) => {
            const { btnId, text, eventCallback, type } = item;
            const btn = document.createElement('button');
            btn.innerHTML = text;
            btn.id = btnId;
            //btn.setAttribute("data-loader", btnId);
            //btn.setAttribute("data-loading-text", `<i class='fa fa-spinner fa-spin'>${text}</i>`);
            btn.classList.add('btn', 'btn-primary', 'button-loader', 'two-tone-button');
            btn.style.minWidth = '8vw';
            btn.style.maxWidth = 'fit-content';
            actionsRow.appendChild(btn);
            if (eventCallback) {
                switch (type) {
                    case 'SEARCH':
                        btn.addEventListener('click', (event) => actionSearchEvent(ctrlId, eventCallback, event, params));
                        //actionSearchEvent(ctrlId, eventName);
                        break;
                    case 'CLEAR':
                        btn.addEventListener('click', (event) => clearSearchEvent(ctrlId, eventCallback, event, params));
                        //btn.addEventListener('click', eventName);
                        break;
                    default:
                        btn.addEventListener('click', eventCallback, params);
                        break;
                }

                //document.getElementById(id).addEventListener('click', eventName);
                /*btn.addEventListener('click', (event) => {
                    $('#event-search-column, .search-control').each(function () {
                        console.log('ITEM', $(this).val());
                    });
                    eventName(event, )
                });*/
            }
        });

        const main = document.createElement('div');
        main.innerHTML = null;
        main.id = ctrlId;
        main.classList.add('search-column-container', 'full-column-row-gap');
        const searchTitle = document.createElement('span');
        //searchTitle.innerHTML = 'لوحة البحث';
        searchTitle.classList.add('search-title');
        main.appendChild(searchTitle);
        main.appendChild(controlsRow);
        main.appendChild(actionsRow);

        if (appendId) {
           const appendElem =  document.getElementById(appendId);
            appendElem.appendChild(main);
        }
        return main;
    };

    const datepickerRangeValidation =(startDateKey, endDateKey) => {
        const startElem = document.querySelector(`[data-key="${startDateKey}"]`);
        const endElem = document.querySelector(`[data-key="${endDateKey}"]`);
        const stDateId = startElem.getAttribute('id');
        const edDateId = endElem.getAttribute('id');
        commonUtil.dateRangeValidation(stDateId, edDateId);
    };



    let result = {};
    result.createColumnSearch = createColumnSearch;
    result.datepickerRangeValidation = datepickerRangeValidation;

    return result;
})();

