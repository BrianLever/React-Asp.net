import React from 'react';
import CssrsCheckbox from 'components/UI/checkbox/CssrsCheckbox';


export type TRiskAssessmentCheckboxItemProps = {
    name: string;
    id: string;
    isChecked: boolean | undefined;
    changeHandler?: (v: any) => void;
    description: string;
}

const RiskAssessmentCheckboxItem = (props: TRiskAssessmentCheckboxItemProps): React.ReactElement => {
    const { id, name, isChecked, changeHandler, description } = props;
    return (
        <div style={{ display: 'flex' }}>
            <CssrsCheckbox 
                id={id}
                name={name}
                isChecked={isChecked}
                changeHandler={changeHandler}
                style={{ cursor: 'pointer' }}
            />
            <p style={{ marginLeft: 15 }}>{description}</p>
        </div>
    )
}

export default RiskAssessmentCheckboxItem;