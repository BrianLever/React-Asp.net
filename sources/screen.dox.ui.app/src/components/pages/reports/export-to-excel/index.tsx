import React, { useEffect } from 'react';

import { TitleText} from '../../styledComponents';
import {  FormGroup, FormControlLabel, Checkbox
} from '@material-ui/core';

import { reportsInitState } from 'states/reports';
import { useDispatch } from 'react-redux';
import { postFilteredIncludeCombinedRequest, postFilteredIncludeDemographicsRequest, postFilteredIncludeDrugsOfChoiceRequest, postFilteredIncludeFollowUpsRequest, postFilteredIncludeScreeningsRequest, postFilteredIncludeVisitsRequest } from 'actions/reports';



const ExportToExcelLists = (): React.ReactElement => {
   
    const dispatch = useDispatch();
       
    const handleChange = (event:any) => {
      if(event.target.name==="checkedA"){                    
          dispatch(postFilteredIncludeScreeningsRequest());
      }
      else if(event.target.name==="checkedB"){
        dispatch(postFilteredIncludeDemographicsRequest());
      }
      else if(event.target.name==="checkedC"){
        dispatch(postFilteredIncludeVisitsRequest());
      }
      else if(event.target.name==="checkedD"){
        dispatch(postFilteredIncludeFollowUpsRequest());
      }
      else if(event.target.name==="checkedE"){
        dispatch(postFilteredIncludeDrugsOfChoiceRequest());
      }
      else if(event.target.name==="checkedF"){
        dispatch(postFilteredIncludeCombinedRequest());
      }
    };      
 
    return ( 
            <div style={{ marginTop: '35px',marginLeft:'40px' }}>      
              <TitleText>
              Export To Excel
              </TitleText>
              <br></br>
              <p> Check each box for data to be exported to Excel.<br></br>
                 Select the location and report period. Click the “Export” button.
              </p>  
              <div style={{ marginTop: '50px',marginLeft:'40px' }}>
                 <FormGroup >
                    <FormControlLabel style={{ pointerEvents: "none" }}
                     control={<Checkbox style={{ pointerEvents: "auto" }}  onChange={handleChange} name="checkedA" color="default"  />}
                       label="Screening Results" labelPlacement="end"/>
                    <FormControlLabel style={{ pointerEvents: "none" }}
                       control={<Checkbox style={{ pointerEvents: "auto" }} onChange={handleChange} name="checkedB" color="default" />}
                      label=" Demographics Report" labelPlacement="end"/>  
                    <FormControlLabel style={{ pointerEvents: "none" }}
                       control={<Checkbox  style={{ pointerEvents: "auto" }}onChange={handleChange} name="checkedC" color="default" />}
                      label=" Visit Report" labelPlacement="end"/>   
                    <FormControlLabel style={{ pointerEvents: "none" }}
                       control={<Checkbox style={{ pointerEvents: "auto" }} onChange={handleChange} name="checkedD" color="default" />}
                      label="Follow-Up Report" labelPlacement="end"/>   
                    <FormControlLabel style={{ pointerEvents: "none" }}
                       control={<Checkbox style={{ pointerEvents: "auto" }} onChange={handleChange} name="checkedE" color="default" />}
                      label="Drug Use Results" labelPlacement="end"/>  
                    <FormControlLabel style={{ pointerEvents: "none" }}
                       control={<Checkbox style={{ pointerEvents: "auto" }} onChange={handleChange} name="checkedF" color="default" />}
                      label="Combined Report (Screening, Drug Use, and Demographics)" labelPlacement="end"/>   
                    {/* <FormControlLabel style={{ pointerEvents: "none" }}
                       control={<Checkbox style={{ pointerEvents: "auto" }} onChange={handleChange} name="checkedG" color="default" />}
                      label="Coronavirus Impact Scale" labelPlacement="end"/>           */}
                  </FormGroup>  
              </div>               
             </div>                   
         
    )
  }
  
  export default ExportToExcelLists;
  