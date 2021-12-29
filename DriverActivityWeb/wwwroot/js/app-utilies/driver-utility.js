const driverUtil = (function (options) {

    let uploadedFiles = []
        ;
    let dialogElem = null, controlsConstrain = [], allConstraint = {}, settingValues = [], fileID = null,
        signaturePad = null
        ;

    const btnSubmitId = 'btn-submit-form',
        overlayContentContainerId = "overlay-content-container",
        fullPopupCloseId = commonUtil.BTN_FULL_POPUP_CLOSE_ID,
        modelBodyId = commonUtil.APP_CONTENT_MODAL_BODY_ID,
        appContentModalId = commonUtil.APP_CONTENT_MODAL_ID,
        updateSectionId = 'view_update_section',
        driverContainerId = 'driver-container',
        tblDriverId = 'tbl-driver',
        NO_DATA = '...',
        driverSignPadId = 'driverSignPad',
        btnClearSignId = 'btnClearSign'
        ;

    let uiConrols = {
        txtDir: "LTR",
        maxLength: 0,
        requireMsgErr: null,
        msgLengthErr: null,
        paginationItem: null,
        btnSubmit: 'Submit',
        ddlPlaceholder: 'Select an option',
        msgFileUpload: 'Drag and drop files here',
        duplicatEentryError: `Duplicate file can't be upload`,
        emptyFileError: `Empty file can't be upload`,
        btnCancel: 'Cancel',
        successMsg: 'Data saved successfully.',
        updateMsg: 'Data updated successfully.',
        varActions: 'Action(s)',
        varView: 'View',
        varEdit: 'Edit'
    };

    const getAttributes = () => {
        return {
            dailogId: commonUtil.CONTENT_DAILOG_ID,
            uploadDropzoneId: "upload-dropzone"
        };
    };

    const getActionDate = (val, outputFormat, inputFormat, dir) => {
        if (!val) return NO_DATA;
        const { txtDir } = driverUtil.uiConrols;
        outputFormat = outputFormat ?? commonUtil.DATE_FORMAT.lll;
        return commonUtil.getLocalUtcDateStringEn(val, outputFormat, inputFormat);
        //if (dir) {
        //    return dir === 'RTL' ? commonUtil.getLocalUtcDateStringAr(val, format) : commonUtil.getLocalUtcDateStringEn(val, format);
        //}
        //return txtDir === 'RTL' ? commonUtil.getLocalUtcDateStringAr(val, format) : commonUtil.getLocalUtcDateStringEn(val, format);
    };

    const getDriverColDef = ({ vehicleList }) => {
        const viewColDef = [
            {
                controlName: 'nameEn',
                header: 'Name EN',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'TEXT_BOX',
                rowOrder: 1,
                columnOrder: 1,
                isRequired: true
            },
            {
                controlName: 'staffID',
                header: 'Staff ID',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'NUMBER_BOX',
                rowOrder: 2,
                columnOrder: 1,
                isRequired: true
            },
            {
                controlName: 'qid',
                header: 'QID',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'NUMBER_BOX',
                rowOrder: 2,
                columnOrder: 2,
                isRequired: true
            },
            {
                controlName: 'vehicleID',
                header: 'Vehicle Type',
                //uibackendName: 'AGENDA_COMPLAINT',
                controlType: 'DROPDOWN',
                rowOrder: 3,
                columnOrder: 1,
                isRequired: true,
                ddlLabelAttr: 'vehicle',
                ddlValueAttr: 'vehicleID',
                collections: vehicleList
            },
            /*{
                controlName: 'driverImage',
                header: 'Driver Image',
                //uibackendName: 'AGENDA_COMPLAINT',
                controlType: 'UPLOAD',
                rowOrder:3,
                columnOrder: 1,
                isRequired: true
            },*/
            {
                controlName: `BTN_SAVE_EVENT_${commonUtil.STATUS_MASTER_TYPE.SUBMIT}`,
                header: 'Submit',
                //uibackendName: 'AGENDA_COMPLAINT',
                controlType: 'BUTTON',
                rowOrder: 4,
                columnOrder: 1,
            },
        ];

        const constraints = formUtilities.setConstraint({ controls: viewColDef });
        return { viewColDef, constraints };
    };

    const getDriverEODColDef = () => {
        const viewColDef = [
            {
                controlName: 'total',
                header: 'Total Delivery',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'NUMBER',
                //rowOrder: 1,
                columnOrder: 1,
            },
            {
                controlName: 'delivered',
                header: 'Delivered',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'NUMBER',
                //rowOrder: 2,
                columnOrder: 1,
            },
            {
                controlName: 'failed',
                header: 'Failed Delivery',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'NUMBER',
                //rowOrder: 3,
                columnOrder: 1,
            },
            {
                controlName: 'drops',
                header: 'No. of Drops',
                //uibackendName: 'AGENDA_ASSIGN_COMPLAINT_REMARKS',
                controlType: 'NUMBER',
                //rowOrder: 4,
                columnOrder: 1,
            },
            //{
            //    controlName: 'driverEODDetailsSection',
            //    //header: 'Driver Sign',
            //    //uibackendName: 'AGENDA_COMPLAINT',
            //    controlType: 'DIV',
            //    //rowOrder: 3,
            //    columnOrder: 1,
            //},
            {
                controlName: 'additionalDelivery',
                header: 'Additional Delivery',
                //uibackendName: 'AGENDA_COMPLAINT',
                controlType: 'NUMBER_BOX',
                //rowOrder: 3,
                columnOrder: 1,
                isRequired: true,
            },
            {
                controlName: 'remarks',
                header: 'Comments',
                //uibackendName: 'AGENDA_COMPLAINT',
                controlType: 'TEXT_AREA',
                //rowOrder: 3,
                columnOrder: 1,
                isRequired: true,
            },
            {
                controlName: 'profilePictureSection',
                header: '',
                //uibackendName: 'AGENDA_COMPLAINT ,
                controlType: 'DIV',
                //rowOrder: 3,
                columnOrder: 2,
            },
            {
                controlName: 'driverSignSection',
                header: 'Driver Sign',
                //uibackendName: 'AGENDA_COMPLAINT ,
                controlType: 'DIV',
                //rowOrder: 3,
                columnOrder: 2,
                isRequired: true,
            },
            //{
            //    controlName: 'driverEODActionSection',
            //    //header: 'Driver Sign',
            //    //uibackendName: 'AGENDA_COMPLAINT ,
            //    controlType: 'DIV',
            //    //rowOrder: 3,
            //    columnOrder: 3,
            //},
            /*{
                controlName: `BTN_SAVE_EVENT_${commonUtil.STATUS_MASTER_TYPE.DRIVER_EOD}`,
                header: 'Submit',
                //uibackendName: 'AGENDA_COMPLAINT',
                controlType: 'BUTTON',
                rowOrder: 2,
                columnOrder: 3
            },*/
        ];

        const constraints = formUtilities.setConstraint({ controls: viewColDef });
        return { viewColDef, constraints };
    };

    const getDriverColumnDefs = () => {
        const { txtDir, varActions } = driverUtil.uiConrols;
        return [
            { "title": "ID", "data": "userId", visible: false },
            {
                "title": varActions, "data": "tblActions", "className": 'dtbl-cell-actions cell-actionlist',
                "width": "13%",
                "render": function (data, type, row) {
                    return driverActionTmpl(data, type, row);
                }
            },
            //{
            //    "title": 'Info', "data": "complaintId",
            //    //"width": "30%",
            //    "render": function (data, type, row) {
            //        const { feedbackId, complaintId } = row;
            //        return `Feedback:${feedbackId} => Complaint: ${complaintId}`;
            //    }
            //},
            {
                "title": 'Staff ID', "data": "staffId", className: 'cell-staffID',
                ////"width": "40%",
                //"render": function (data, type, row) {
                //    const { department } = row;
                //    return txtDir === 'RTL' ? department?.depNameAr : department?.depNameEn;
                //}
            },
            { "title": 'Name (En)', "data": "name", className: "cell-nameEn" },
            { "title": 'QID', "data": "qid", className: "cell-qid" },
            {
                "title": 'Created Date', "data": "createdDate", className: "cell-createdate",
                "width": "15%",
                "render": function (data, type, row) {
                    const { createdDate } = row;
                    return driverUtil.getActionDate(createdDate, commonUtil.DATE_FORMAT.lll, "LRT");
                }
            },
            //{
            //    "title": 'Created By', "data": "createdBy", className: "cell-creator",
            //    "width": "13%",
            //    //"render": function (data, type, row) {
            //    //    const { created } = row;
            //    //    return txtDir === 'RTL' ? created?.name : created?.nameEn;
            //    //}
            //},

        ];
    };

    const getDriverDeliveryColumnDefs = () => {
        return [
            { "title": "ID", "data": "userId", visible: false },
            { "title": 'Staff ID', "data": "staffId", className: 'cell-staffID', visible: false },
            {
                "title": 'Profile Image', "data": "profilePicture",
                className: "has-flex-column has-no-label row-one-column-one text-capitalize cell-profilePicture",
                "width": "12%",
                "render": function (data, type, row) {
                    const { profilePicture } = row;
                    return `<img src="${profilePicture ?? '../images/avatar.png'}" class="cell-avatar" />`;
                }
            },
            { "title": 'Name', "data": "name", className: "has-flex-column has-no-label text-capitalize row-one-column-two cell-nameEn" },
            {
                "title": 'Vehicle', "data": "vehicleName",
                className: "has-flex-column has-no-label row-one-column-two text-capitalize cell-vehicleName",
                "render": function (data, type, row) {
                    const { vehicleName } = row;
                    return `Driver (${vehicleName ?? NO_DATA})`;
                }
            },
            {
                "title": 'Duty Status', "data": "dutyStatus",
                className: "has-flex-column has-no-label row-one-column-two text-capitalize cell-dutyStatus",
                "width": "10%",
                "render": function (data, type, row) {
                    const { dutyStatus } = row;
                    return `<span class="cell-btn-dutyStatus bg-${dutyStatus}"><i class="bi bi-plus fa-fw" aria-hidden="true"></i> ${dutyStatus == 'OnDuty' ? 'ON ROAD' : 'FINISHED'}</span>`;
                }
            },
            {
                "title": 'Total', "data": "total", className: "row-two has-flex-column cell-total",
                "width": "10%",
            },
            {
                "title": 'Delivered', "data": "delivered", className: "row-two has-flex-column cell-delivered",
                "width": "10%",
            },
            {
                "title": 'Failed', "data": "failed", className: "row-two has-flex-column cell-failed",
                "width": "10%",
            },
            {
                "title": 'Drops', "data": "drops", className: "row-two has-flex-column cell-drops",
                "width": "10%",
            },
        ];
    };

    const driverActionTmpl = (data, type, row) => {
        const { userId, staffID, } = row;
        const pkId = userId;

        const template = `
                <section class="sec-center0" id="action__section__${pkId}" data-key="${pkId}">
                    <div style="display:flex; align-items: center;">
                       
                        <button class="view btn btn-primary complaint-view" title='View'>
                            <i class="view bi bi-eye"></i>
                        </button>
                        <button class="view btn btn-primary complaint-view" title='View'>
                            <i class="view bi bi-pencil"></i>
                        </button>

                    </div>
                </section>
                `;
        return template;
    };

    const handleReviewFormSubmit = (form, formConstraints, colDef, extraValidationCntrolsName) => {
        let errors = validate(form, formConstraints);
        let controlsName = [];
        if (colDef) {
            const allCols = _.flattenDeep(colDef?.map(x => x.column));
            const editorColsDef = allCols?.filter(x => x.type === 'TEXT_EDITOR');
            if (editorColsDef && editorColsDef.length > 0) {
                editorColsDef.forEach(col => {
                    const editorId = `text-editor-${col.field}`;
                    const constraint = formConstraints[editorId];
                    if (!_.isEmpty(constraint)) {
                        controlsName.push(editorId);
                        const editorVal = $(`#${col.field}`).closest(".jqte").find(".jqte_editor").eq(0).text();
                        let ctrlError = {};
                        const presenceConstraint = constraint['presence'];
                        if (presenceConstraint) {
                            ctrlError = {
                                ...ctrlError,
                                presence: presenceConstraint['message']
                            };
                            if (editorVal && editorVal?.trim()?.length > 0)
                                delete ctrlError['presence'];
                        }
                        const lengthConstraint = constraint['length'];
                        if (editorVal && editorVal?.trim()?.length > 0 && lengthConstraint && lengthConstraint['minimum']) {
                            ctrlError = {
                                ...ctrlError,
                                minimum: lengthConstraint['tooShort']
                            };
                            if (editorVal?.length > Number(lengthConstraint['minimum']))
                                delete ctrlError['minimum'];
                        }
                        if (editorVal && editorVal?.trim().length > 0 && lengthConstraint && lengthConstraint['maximum']) {
                            ctrlError = {
                                ...ctrlError,
                                maximum: lengthConstraint['tooLong']
                            };
                            if (editorVal?.length < Number(lengthConstraint['maximum']))
                                delete ctrlError['maximum'];
                        }
                        if (_.isEmpty(ctrlError)) {
                            delete errors[editorId];
                        } else {
                            const errMsg = Object.values(ctrlError)[0];
                            errors = {
                                ...errors,
                                [editorId]: errMsg ? [errMsg.replace('^', '')] : ''
                            };
                        }
                    }
                });
            }
            const uploadColDef = allCols?.filter(x => x.controlType === 'UPLOAD');
            if (uploadColDef && uploadColDef.length > 0) {
                uploadColDef.forEach(col => {
                    const ctrlId = `${col.field}`;
                    const constraint = formConstraints[ctrlId];
                    if (!_.isEmpty(constraint)) {
                        controlsName.push(ctrlId);
                        const ctrlVal = Dropzone.forElement(`#${ctrlId}`).files?.length;
                        let ctrlError = {};
                        const presenceConstraint = constraint['presence'];
                        if (presenceConstraint) {
                            ctrlError = {
                                ...ctrlError,
                                presence: presenceConstraint['message']
                            };
                            if (ctrlVal && ctrlVal > 0)
                                delete ctrlError['presence'];
                        }
                        if (_.isEmpty(ctrlError)) {
                            delete errors[ctrlId];
                        } else {
                            const errMsg = Object.values(ctrlError)[0];
                            errors = {
                                ...errors,
                                [ctrlId]: errMsg ? [errMsg.replace('^', '')] : ''
                            };
                        }
                    }
                });
            }

            if (extraValidationCntrolsName && extraValidationCntrolsName.length > 0) {
                extraValidationCntrolsName.forEach(col => {
                    const { field, selectedValue, selectedField, checkTrue } = col;
                    const ctrlId = `${field}`;
                    const constraint = formConstraints[ctrlId];
                    if (!_.isEmpty(constraint)) {
                        controlsName.push(ctrlId);
                        let ctrlVal = null;
                        if (selectedValue)
                            ctrlVal = eval(selectedValue);
                        else if (selectedField)
                            ctrlVal = $(`#${field}`).val();
                        else if (checkTrue)
                            ctrlVal = eval(checkTrue);

                        let ctrlError = {};
                        const presenceConstraint = constraint['presence'];
                        if (presenceConstraint) {
                            ctrlError = {
                                ...ctrlError,
                                presence: presenceConstraint['message']
                            };

                            if (!_.isEmpty(ctrlVal)) {
                                if (_.isArray(ctrlVal)) {
                                    const arrVal = _.compact(ctrlVal);
                                    if (!_.isEmpty(arrVal)) {
                                        delete ctrlError['presence'];
                                    }
                                }
                                else {
                                    delete ctrlError['presence'];
                                }
                            }
                            else if (checkTrue && eval(checkTrue)) {
                                if (eval(checkTrue))
                                    delete ctrlError['presence'];
                            }
                        }
                        if (_.isEmpty(ctrlError)) {
                            delete errors[ctrlId];
                        } else {
                            const errMsg = Object.values(ctrlError)[0];
                            errors = {
                                ...errors,
                                [ctrlId]: errMsg ? [errMsg.replace('^', '')] : ''
                            };
                        }
                    }
                });
            }
        }
        formUtilities.showErrors(form, errors, controlsName);
        console.log('errors', errors);
        if (_.isEmpty(errors)) return true;
        else return false;

    };

    const resizeCanvas = (canvasId) => {
        if (!canvasId) return;
        canvas = document.getElementById(canvasId);
        const ratio = Math.max(window.devicePixelRatio || 1, 1);
        canvas.width = canvas.offsetWidth * ratio;
        canvas.height = canvas.offsetHeight * ratio;
        canvas.getContext("2d").scale(ratio, ratio);
    };

    const openSignatureWindow = (base64URL) => {
        const win = window.open();
        win.document.write('<iframe src="' + base64URL + '" frameborder="0" style="border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;" allowfullscreen></iframe>');
    };

    const saveFormData = ({ reqData, btnEventObj, url }) => {
        const { btnId, liId } = btnEventObj;
        const eventKey = liId.replace('EVENT_', '');
        //commonUtil.btnProgress(btnId);
        jqClient.Post({
            url: url ?? "/Home/SaveFormData",
            jsonData: reqData,
            btnLoaderId: btnId,
            config: {
                dataType: 'json'
            },
            onSuccess: (response) => {
                try {
                    const { data } = response;
                    const formId = `FORM_${liId}`;
                    const formElem = formUtilities.getFormElem(formId);
                    if (formElem)
                        formElem.reset();

                    if (commonUtil.STATUS_MASTER_TYPE.SUBMIT == eventKey) {
                    }
                    else if (commonUtil.STATUS_MASTER_TYPE.DRIVER_EOD == eventKey) {
                        signaturePad.clear();
                    }
                    if (data) {
                    }
                } catch (e) { }
                finally {
                    const { successMsg } = driverUtil.uiConrols;
                    notificationUtil.success(successMsg);
                }
            }
        });
    };

    const formSubmitEvent = (btnEventObj) => {
        const { btnId, eventName } = btnEventObj;
        const btnInput = document.querySelector(`#${btnId}`);
        btnInput.myParam = btnEventObj;
        document.getElementById(btnId).addEventListener(eventName, ev => {
            const btnEventObjItem = ev.currentTarget?.myParam;
            if (btnEventObjItem) {
                const { liId, formConstraints, colDef, extraValidationCntrolsName, data } = btnEventObjItem;
                const eventKey = liId.replace('EVENT_', '');
                const formId = `FORM_${liId}`;
                const formValues = formUtilities.getFormControlValue(formId);
                const formElem = formUtilities.getFormElem(formId);

                const isValid = handleReviewFormSubmit(formElem, formConstraints, colDef, extraValidationCntrolsName);
                if (!isValid) return;

                if (eventKey === commonUtil.STATUS_MASTER_TYPE.SUBMIT) {
                    if (!driverUtil.fileID)
                        return notificationUtil.popup({ title: "Error", body: "Kindly upload the profile picture." });
                }
                let reqData = formValues;
                /*reqData = {
                    ...reqData,
                    currentStatus: eventKey
                };*/
                let url = btnEventObj.url;
                if (commonUtil.STATUS_MASTER_TYPE.SUBMIT == eventKey) {
                    reqData = {
                        ...reqData,
                        fileID: driverUtil.fileID
                    };
                }
                else if (commonUtil.STATUS_MASTER_TYPE.DRIVER_EOD == eventKey) {
                    reqData = {
                        ...reqData,
                        signature: signaturePad.toDataURL('image/png'),
                        delivered: data?.delivered,
                        drops: data?.drops,
                        failedDelivery: data?.failed,
                        totalDelivery: data?.total,
                        staffId: data?.staffId,
                        userId: data?.userId,
                    };
                }

                saveFormData({ reqData, btnEventObj, url });
            }

        }, false);
    };

    const createDriverForm = ({ eventId, title, rowData, extraValidationCntrolsName, vehicleList, columnsDef, url }) => {
            extraValidationCntrolsName = extraValidationCntrolsName ?? [],
            rowData = rowData ?? {}
            ;
        const eventKey = eventId.replace('EVENT_', '');
        switch (eventKey) {
            case commonUtil.STATUS_MASTER_TYPE.DRIVER_EOD:
                columnsDef = getDriverEODColDef();
                url = commonUtil.APP_URL.SAVE_DRIVER_EOD_FORM;
                const numberBoxs = columnsDef?.viewColDef?.find(x => x.controlType === 'NUMBER_BOX');
                if (numberBoxs && numberBoxs?.length > 0) {
                    numberBoxs.forEach(x => {
                        x.isNumnerFormat = true;
                    });
                }
                let profilePictureSectionCtrl = columnsDef?.viewColDef?.find(x => x.controlName === 'profilePictureSection');
                let driverSignSectionCtrl = columnsDef?.viewColDef?.find(x => x.controlName === 'driverSignSection');
                let eodRemarksCtrl = columnsDef?.viewColDef?.find(x => x.controlName === 'remarks');
                if (profilePictureSectionCtrl) {
                    profilePictureSectionCtrl.isCellMsg = false;
                    profilePictureSectionCtrl.isLabel = false;
                }
                if (eodRemarksCtrl) {
                    eodRemarksCtrl.rows = 4;
                }

                if (driverSignSectionCtrl) {
                    driverSignSectionCtrl = {
                        ...driverSignSectionCtrl,
                        field: driverSignSectionCtrl.controlName,
                        type: driverSignSectionCtrl.controlType,
                        checkTrue: '!signaturePad.isEmpty()'
                    }
                    //driverSignSectionCtrl['selectedField'] = officerColDef.field;
                    extraValidationCntrolsName.push(driverSignSectionCtrl);
                }
                break;
            case commonUtil.STATUS_MASTER_TYPE.SUBMIT:
                columnsDef = getDriverColDef({ vehicleList });
                url = commonUtil.APP_URL.SAVE_DRIVER_FORM;
                break;
            default:
                break;
        }

        const { viewColDef: colDef, constraints } = columnsDef;

        if (!colDef || colDef?.length <= 0)
            return notificationUtil.warning("column(s) defination not found.");

        const { controlsDef, isRowColView } = formUtilities.createColDef({ controls: colDef });
        const allCols = _.flattenDeep(controlsDef?.map(x => x.column));

        const ctrlId = `FORM_${eventId}`;
        const formDef = {
            ctrlId,
            textDirection: uiConrols.txtDir,
            isRowColView: isRowColView,
            jsonData: rowData,
            colDef: controlsDef
        };

        let btnEventObj = {};
        const submitBtnColDef = allCols.find(x => x.controlType === 'BUTTON');
        if (submitBtnColDef) {
            btnEventObj = {
                ...btnEventObj,
                btnId: `BTN_SAVE_${eventId}`,
                btnLabel: uiConrols.btnSubmit,
                eventName: 'click',
                title,
                liId: `${eventId}`,
                formConstraints: constraints,
                data: rowData,
                url,
                colDef: controlsDef,
                extraValidationCntrolsName
            };
        }

        const body = formUtilities.createViewWindow(formDef);
        document.querySelector(`#${driverContainerId}`).append(body);
        const formElem = formUtilities.getFormElem(ctrlId);

        if (eventKey === commonUtil.STATUS_MASTER_TYPE.DRIVER_EOD) {
            const driverSignCtrl = allCols?.find(x => x.controlName === 'driverSignSection');
            const profilePictureSectionCtrl = allCols?.find(x => x.controlName === 'profilePictureSection');
            if (driverSignCtrl) {
                const canvas = document.createElement('canvas');
                canvas.id = driverUtil.driverSignPadId;
                canvas.classList.add('driver-signature-pad')
                canvas.setAttribute("width", '600');
                canvas.setAttribute("height", '300');

                const driverSignSectionElem = document.getElementById(driverSignCtrl.controlName);
                const btnClearSign = document.createElement('button');
                btnClearSign.innerText = "Clear Sign";
                btnClearSign.id = btnClearSignId;
                btnClearSign.classList.add('btn', 'btn-secondary');
                driverSignSectionElem.append(canvas);
                driverSignSectionElem.append(btnClearSign);
                signaturePad = new SignaturePad(canvas, {
                    backgroundColor: 'rgb(250,250,250)'
                });

                btnClearSign.addEventListener('click', clearSignEvet => {
                    clearSignEvet.preventDefault();
                    signaturePad.clear();
                });
            }

            if (profilePictureSectionCtrl) {
                const profilePictureSectionElm = document.getElementById(profilePictureSectionCtrl.controlName);
                if (profilePictureSectionElm) {
                    const { profilePicture, staffId, viewCreatedDate, name } = rowData ?? {};
                    profilePictureSectionElm.innerHTML = "";
                    profilePictureSectionElm.classList.add('edo-profile-section');
                    const profileTemplate = ` <div class="q-column">
                                                  <div class="profile-pic-after-gradiant">
                                                    <img src="${profilePicture ?? '../images/avatar.png'}" />
                                                  </div>                                                                
                                              </div>
                                              <div class="q-column eod-profile-details">
                                                <div>${name ?? NO_DATA} </div>
                                                <div>${staffId ?? NO_DATA}</div>
                                                <div>${driverUtil.getActionDate(new Date(viewCreatedDate), commonUtil.DATE_FORMAT.ll) ?? NO_DATA} </div>
                                              </div>`;
                    profilePictureSectionElm.innerHTML = profileTemplate;
                }
            }

            if (formElem) {
                const eodBtnSubmitColDef = {
                    field: `BTN_SAVE_EVENT_${commonUtil.STATUS_MASTER_TYPE.DRIVER_EOD}`,
                    header: 'Save & Approve',
                    //uibackendName: 'AGENDA_COMPLAINT',++++
                    type: 'BUTTON',
                };
                btnEventObj = {
                    ...btnEventObj,
                    btnId: `BTN_SAVE_${eventId}`,
                    btnLabel: eodBtnSubmitColDef.header,
                    eventName: 'click',
                    title,
                    liId: `${eventId}`,
                    formConstraints: constraints,
                    data: rowData,
                    url,
                    colDef: controlsDef,
                    extraValidationCntrolsName
                };
                const eodBtnSubmitCtrl = formUtilities.getControl({ colDef: eodBtnSubmitColDef, rowId: 100, textDirection: uiConrols.txtDir });
                if (eodBtnSubmitCtrl) {
                    formElem.append(eodBtnSubmitCtrl);
                }
            }
            /*
             <div class="flex-column">
            <div class="wrapper">
                <canvas id="signature-pad" width="600" height="300"></canvas>
            </div>
            <div class="sign-label">Sign above</div>
            <div class="flex-row action-row">
                <button type="button" id="btn-clear" class="button btn-clear">Clear </button>
                <button type="button" id="btn-save" class="button btn-save">Save </button>
            </div>
        </div>
             
             */
        }

        formUtilities.createFormEvents(formDef.ctrlId, constraints);

        if (!_.isEmpty(btnEventObj))
            formSubmitEvent(btnEventObj);
    };

    const getDriverData = ({ url, reqData, btnId }, callback) => {
        jqClient.Get({
            url: url ?? commonUtil.APP_URL.LOAD_DRIVER_DATA,
            dataType: 'text',
            btnLoaderId: btnId,
            jsonData: reqData,
            onSuccess: (response) => {
                if (callback)
                   return callback(response);
            }
        });
    };

    const getUserSignature = (id) => {
        jqClient.Get({
            url: commonUtil.APP_URL.GET_USER_SIGNATURE,
            dataType: 'text',
            jsonData: {id},
            onSuccess: (response) => {
                const { data } = response;
                driverUtil.openSignatureWindow(data);
            }
        });
    };

    const createDriverTabel = (columnsDef) => {
        const gridElem = document.querySelector(`#${tblDriverId}`);
        const elemId = gridElem.getAttribute('id');
        //const columns = getDriverColumnDefs();
        tableUtil.createDataTable({
            id: elemId, columns: columnsDef,
            //dataSet,
            //paginationConfig: complaintUtil.paginationConfig(),
            config: {
                //scrollY: '50vh',
                //scrollCollapse: true,
                paging: true,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "bFilter": false,
                "bInfo": true,
                order: [[1, 'asc']],
                //rowCallback: function (row, data) {
                //    // Set the checked state of the checkbox in the table
                //    $('input.editor-active', row).prop('checked', data.isChecked == 1);
                //}
                //buttons: tableUtil.getDataTableButton(elemId),
                //drawCallback: tableUtil.drawCallBack()
                'dom':
                    "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'<'float-md-right ml-2 change-view'B>f>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                'buttons': [{
                    'text': '<i class="bi bi-card-list fa-fw" aria-hidden="true"></i>',
                    'action': function (e, dt, node) {
                        $(dt.table().node()).toggleClass('cards');
                        $('.bi', node).toggleClass(['bi-table', 'bi-card-list']);
                        dt.draw('page');
                    },
                    'className': 'btn-sm btn-change-view',
                    'attr': {
                        'title': 'Change View',
                        'data-toggle': 'tooltip'
                    }
                }],
                'select': 'single',
                'drawCallback': function (settings) {
                    tableUtil.drawCallBack(this, settings);
                }
                //'drawCallback': function (settings) {
                //    var api = this.api();
                //    var $table = $(api.table().node());

                //    if ($table.hasClass('cards')) {

                //        // Create an array of labels containing all table headers
                //        var labels = [];
                //        $('thead th', $table).each(function () {
                //            labels.push($(this).text());
                //        });

                //        // Add data-label attribute to each cell
                //        $('tbody tr', $table).each(function () {
                //            $(this).find('td').each(function (column) {
                //                $(this).attr('data-label', labels[column]);
                //            });
                //        });

                //        var max = 0;
                //        $('tbody tr', $table).each(function () {
                //            max = Math.max($(this).height(), max);
                //        }).height(max);

                //    } else {
                //        // Remove data-label attribute from each cell
                //        $('tbody td', $table).each(function () {
                //            $(this).removeAttr('data-label');
                //        });

                //        $('tbody tr', $table).each(function () {
                //            $(this).height('auto');
                //        });
                //    }
                //}
            }
        }, rowEvent => {

            const { element, rowData, selectionChecked } = rowEvent;
            const { userId } = rowData;
            //if (selectionChecked) {
            //    const findObj = selectedOfficers.find(x => x.complaintOfficerId == complaintOfficerId);
            //    if (!findObj)
            //        selectedOfficers.push(rowData);
            //}
            //else {
            //    selectedOfficers = selectedOfficers.filter(x => x.complaintOfficerId != complaintOfficerId);
            //}

            //console.log('selectedOfficers', selectedOfficers);

        });
    };

    let result = {};

    result.uiConrols = uiConrols;
    result.driverSignPadId = driverSignPadId;
    result.getActionDate = getActionDate;
    result.createDriverForm = createDriverForm;
    result.createDriverTabel = createDriverTabel;
    result.getDriverData = getDriverData;
    result.getUserSignature = getUserSignature;
    //result.addEventsToActionRow = addEventsToActionRow;
    //result.openDropzone = openDropzone;
    result.getAttributes = getAttributes;
    result.settingValues = settingValues;
    result.fileID = fileID;
    result.getDriverColumnDefs = getDriverColumnDefs;
    result.getDriverDeliveryColumnDefs = getDriverDeliveryColumnDefs;
    result.resizeCanvas = resizeCanvas;
    result.openSignatureWindow = openSignatureWindow;

    return result;

})();