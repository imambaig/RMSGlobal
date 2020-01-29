﻿import React from 'react'
import { FieldRenderProps } from 'react-final-form'
import { FormFieldProps, FormField, Label } from 'semantic-ui-react'
import { DateTimePicker } from 'react-widgets'

interface IProps extends FieldRenderProps<Date, HTMLElement>, FormFieldProps { }
const DateInput: React.FC<IProps> = ({ input, width, placeholder, date=false,time=false,meta: { touched, error },...rest}) => {
    return <FormField error={touched && !!error} width={width}>
        <DateTimePicker
            placeholder={placeholder}
            value={input.value || null}
            onChange={input.onChange}
            onBlur={input.onBlur}
            onKeyDown={(e)=>e.preventDefault()}
            date={date}
            time={time}
        />
        {touched && error && (
            <Label basic color='red'>
                {error}
            </Label>
        )}
    </FormField>
}

export default DateInput