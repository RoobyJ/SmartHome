<template>
    <v-container fluid class="pa-0">
        <v-row dense>
            <v-col cols="12" class="text-h5">FIELDS WITH DROPDOWNS - {{ title }}</v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-select
                    :items="countries"
                    :density="density"
                    label="v-select"
                    hint="v-select pozwala wybrać tylko elementy z menu"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-autocomplete
                    :items="countries"
                    :density="density"
                    label="v-autocomplete"
                    hint="v-autocomplete pozwala dodatkowo wyszukiwać elementy z menu"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-combobox
                    :items="countries"
                    :density="density"
                    label="v-combobox"
                    hint="v-combobox pozwala wyszukiwać oraz dodawać wartości nie znajdujące się w menu"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-combobox
                    :items="countries"
                    :density="density"
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    clearable
                    label="v-combobox with icons + clearable"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-combobox
                    :items="countries"
                    :density="density"
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    clearable
                    error
                    error-messages="Validation message"
                    label="v-combobox with icons + clearable"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-combobox
                    :items="countries"
                    :density="density"
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    readonly
                    label="v-combobox with icons + readonly"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-combobox
                    :items="countries"
                    :density="density"
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    disabled
                    label="v-c"
                />
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import type { PropType } from 'vue';
import { ref } from 'vue';

defineProps({
    density: { type: String as PropType<'default' | 'comfortable'>, default: 'default' },
    title: { type: String, required: true },
});

const countries = ref([
    'Afghanistan',
    'Albania',
    'Algeria',
    'Andorra',
    'Angola',
    'Antigua & Deps',
    'Argentina',
    'Armenia',
    'Australia',
    'Austria',
    'Azerbaijan',
    'Bahamas',
    'Bahrain',
    'Bangladesh',
    'Barbados',
    'Belarus',
    'Belgium',
    'Belize',
    'Benin',
    'Bhutan',
    'Bolivia',
    'Bosnia Herzegovina',
    'Botswana',
    'Brazil',
    'Brunei',
    'Bulgaria',
    'Burkina',
    'Burundi',
    'Cambodia',
    'Cameroon',
    'Canada',
    'Cape Verde',
    'Central African Rep',
    'Chad',
    'Chile',
    'China',
    'Colombia',
    'Comoros',
    'Congo',
    'Congo {Democratic Rep}',
    'Costa Rica',
    'Croatia',
    'Cuba',
    'Cyprus',
    'Czech Republic',
    'Denmark',
    'Djibouti',
    'Dominica',
    'Dominican Republic',
    'East Timor',
    'Ecuador',
    'Egypt',
    'El Salvador',
    'Equatorial Guinea',
    'Eritrea',
    'Estonia',
    'Ethiopia',
    'Fiji',
    'Finland',
    'France',
    'Gabon',
    'Gambia',
    'Georgia',
    'Germany',
    'Ghana',
    'Greece',
    'Grenada',
    'Guatemala',
    'Guinea',
    'Guinea-Bissau',
    'Guyana',
    'Haiti',
    'Honduras',
    'Hungary',
    'Iceland',
    'India',
    'Indonesia',
    'Iran',
    'Iraq',
    'Ireland {Republic}',
    'Israel',
    'Italy',
    'Ivory Coast',
    'Jamaica',
    'Japan',
    'Jordan',
    'Kazakhstan',
    'Kenya',
    'Kiribati',
    'Korea North',
    'Korea South',
    'Kosovo',
    'Kuwait',
    'Kyrgyzstan',
    'Laos',
    'Latvia',
    'Lebanon',
    'Lesotho',
    'Liberia',
    'Libya',
    'Liechtenstein',
    'Lithuania',
    'Luxembourg',
    'Macedonia',
    'Madagascar',
    'Malawi',
    'Malaysia',
    'Maldives',
    'Mali',
    'Malta',
    'Marshall Islands',
    'Mauritania',
    'Mauritius',
    'Mexico',
    'Micronesia',
    'Moldova',
    'Monaco',
    'Mongolia',
    'Montenegro',
    'Morocco',
    'Mozambique',
    'Myanmar, {Burma}',
    'Namibia',
    'Nauru',
    'Nepal',
    'Netherlands',
    'New Zealand',
    'Nicaragua',
    'Niger',
    'Nigeria',
    'Norway',
    'Oman',
    'Pakistan',
    'Palau',
    'Panama',
    'Papua New Guinea',
    'Paraguay',
    'Peru',
    'Philippines',
    'Poland',
    'Portugal',
    'Qatar',
    'Romania',
    'Russian Federation',
    'Rwanda',
    'St Kitts & Nevis',
    'St Lucia',
    'Saint Vincent & the Grenadines',
    'Samoa',
    'San Marino',
    'Sao Tome & Principe',
    'Saudi Arabia',
    'Senegal',
    'Serbia',
    'Seychelles',
    'Sierra Leone',
    'Singapore',
    'Slovakia',
    'Slovenia',
    'Solomon Islands',
    'Somalia',
    'South Africa',
    'South Sudan',
    'Spain',
    'Sri Lanka',
    'Sudan',
    'Suriname',
    'Swaziland',
    'Sweden',
    'Switzerland',
    'Syria',
    'Taiwan',
    'Tajikistan',
    'Tanzania',
    'Thailand',
    'Togo',
    'Tonga',
    'Trinidad & Tobago',
    'Tunisia',
    'Turkey',
    'Turkmenistan',
    'Tuvalu',
    'Uganda',
    'Ukraine',
    'United Arab Emirates',
    'United Kingdom',
    'United States',
    'Uruguay',
    'Uzbekistan',
    'Vanuatu',
    'Vatican City',
    'Venezuela',
    'Vietnam',
    'Yemen',
    'Zambia',
    'Zimbabwe',
]);
</script>
