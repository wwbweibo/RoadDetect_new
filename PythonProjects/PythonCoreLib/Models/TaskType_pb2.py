# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: TaskType.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf.internal import enum_type_wrapper
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='TaskType.proto',
  package='PythonCoreLib.Models',
  syntax='proto3',
  serialized_options=None,
  serialized_pb=_b('\n\x0eTaskType.proto\x12\x14PythonCoreLib.Models*:\n\x08TaskType\x12\x0f\n\x0b\x44\x61taCollect\x10\x00\x12\x0e\n\nPreProcess\x10\x01\x12\r\n\tCrackCalc\x10\x02\x62\x06proto3')
)

_TASKTYPE = _descriptor.EnumDescriptor(
  name='TaskType',
  full_name='PythonCoreLib.Models.TaskType',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='DataCollect', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='PreProcess', index=1, number=1,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='CrackCalc', index=2, number=2,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=40,
  serialized_end=98,
)
_sym_db.RegisterEnumDescriptor(_TASKTYPE)

TaskType = enum_type_wrapper.EnumTypeWrapper(_TASKTYPE)
DataCollect = 0
PreProcess = 1
CrackCalc = 2


DESCRIPTOR.enum_types_by_name['TaskType'] = _TASKTYPE
_sym_db.RegisterFileDescriptor(DESCRIPTOR)


# @@protoc_insertion_point(module_scope)
